using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using OfisUrunTakip.WebApi.Services;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OfisUrunTakip.WebApi.Workers
{
    public class LowStockMailWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LowStockMailWorker> _logger;

    
        private static HashSet<int> _sentUserIds = new HashSet<int>();//bugn gonderılen kısılerın ıd sını tutuyor 
        private static DateTime _lastRunDate = DateTime.Today;//gun degısınce cache  temızlenıyor 

        public LowStockMailWorker(IServiceScopeFactory scopeFactory, ILogger<LowStockMailWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // mail gonderılen kullnıı sıfırlanır
                    //yenı gune hazur
                    if (DateTime.Today != _lastRunDate)
                    {
                        _sentUserIds.Clear();
                        _lastRunDate = DateTime.Today;
                        Console.WriteLine(" Yeni gün başladı, cache temizlendi.");
                    }

                    Console.WriteLine($"\n[KONTROL] {DateTime.Now:HH:mm:ss} - Veritabanı taranıyor...");

                    // HER DÖNGÜDE YENİ SCOPE OLUŞTUR
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApiContext>();
                        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                        var lowStockProducts = await db.Products
                            .Include(p => p.Category)
                            .Where(p => !p.IsDeleted && p.CurrentStock <= p.ReorderLevel)
                            .OrderBy(p => p.CurrentStock)
                            .ToListAsync(stoppingToken);
                        //stogu azalan urunler gelır 

                        if (lowStockProducts.Count > 0)
                        {
                            Console.WriteLine($"    {lowStockProducts.Count} ürün stokta düşük!");

                            // //kım maıl almak ıstıor hangı sıklıkla ıstıyor
                            var activeSettings = await db.UserEmailSettings
                                .Where(s => s.IsActive == true)
                                .ToListAsync(stoppingToken);

                            Console.WriteLine($"    Aktif {activeSettings.Count} kullanıcı bulundu.");

                            // DEBUG: Hangi kullanıcılar aktif görelim
                            foreach (var s in activeSettings)
                            {
                                Console.WriteLine($"      - UserID: {s.UserId}, IsActive: {s.IsActive}");
                            }

                            foreach (var setting in activeSettings)
                            {
                                // Bugün bu kullanıcıya zaten mail attık mı?
                                if (_sentUserIds.Contains(setting.UserId))
                                {
                                    Console.WriteLine($"    ATLANIYOR: UserID {setting.UserId} (Bugün zaten mail gönderildi)");
                                    continue;
                                }

                                var user = await db.Users.FindAsync(setting.UserId);
                                if (user == null)
                                {
                                    Console.WriteLine($"    ATLANIYOR: UserID {setting.UserId} (Kullanıcı bulunamadı)");
                                    continue;
                                }

                             
                                bool shouldSend = ShouldSendToUser(setting);

                                Console.WriteLine($"   ?? GÖNDERİM BAŞLIYOR: {user.Name}");
                                Console.WriteLine($"      - IsActive: {setting.IsActive}");
                                Console.WriteLine($"      - Frequency: {setting.Frequency}");
                                Console.WriteLine($"      - ShouldSend: {shouldSend}");

                                if (shouldSend)
                                {
                                   
                                    var rows = string.Join("", lowStockProducts.Select(p =>
                                        $"<tr><td>{p.Name}</td><td>{p.Category?.Name}</td><td>{p.CurrentStock}</td><td>{p.ReorderLevel}</td></tr>"
                                    ));

                                    var body = $@"<h2>Sayın {user.Name},</h2>
                                        <p>Günlük stok kontrol raporunuz:</p>
                                        <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse; width:100%;'>              
                                          <thead style='background-color:#f2f2f2;'>
                                            <tr>
                                              <th>Ürün</th>
                                              <th>Kategori</th>
                                              <th>Stok</th>
                                              <th>Min. Sınır</th>
                                            </tr>
                                          </thead>
                                          <tbody>{rows}</tbody>
                                        </table>
                                        <p>Bu mail günde sadece 1 kez gönderilir.</p>";

                                    try
                                    {
                                        await emailSender.SendAsync(user.Email, $"Stok Raporu ({DateTime.Now:dd.MM.yyyy})", body, stoppingToken);

                                        Console.WriteLine($"   MAİL GİTTİ: {user.Email}");

                                        // Hafızaya kaydet
                                        _sentUserIds.Add(setting.UserId);

                                        // DB Logu
                                        db.EmailNotifications.Add(new EmailNotification
                                        {
                                            UserId = user.Id,
                                            Status = Status.Sent,
                                            Message = $"Günlük Rapor: {lowStockProducts.Count} ürün.",
                                            SentDate = DateTime.Now
                                        });

                                        await db.SaveChangesAsync(stoppingToken);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"    MAİL HATASI: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"    ATLANIYOR: {user.Name} (Bugün gönderim günü değil)");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("   Stok sorunu yok, kimseye mail atılmadı.");
                        }
                    } // using scope sonu - dispose oluyor
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" GENEL HATA: {ex.Message}");
                    _logger.LogError(ex, "Worker hatası");
                }

                Console.WriteLine(" 1 Dakika bekleniyor...\n");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private static bool ShouldSendToUser(UserEmailSetting setting)
        {
            if (setting == null) return false;

            var today = DateTime.Now;

            if (setting.Frequency == "Daily") return true;

            if (setting.Frequency == "Weekly")
            {
                int currentDay = (int)today.DayOfWeek;
                if (currentDay == 0) currentDay = 7;
                string days = string.IsNullOrEmpty(setting.Days) ? "1" : setting.Days;
                if (days.Contains(currentDay.ToString())) return true;
            }

            if (setting.Frequency == "Monthly")
            {
                int dayOfMonth = today.Day;
                if (!string.IsNullOrEmpty(setting.Days) && setting.Days.Contains(dayOfMonth.ToString())) return true;
            }

            return false;
        }
    }
}