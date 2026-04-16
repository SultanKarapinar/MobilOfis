using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using OfisUrunTakip.WebApi.DTO; 
using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace OfisUrunTakip.WebApi.Controllers
{
    [ApiController]
    [Route("api/EmailSettings")] // React buraya istek atacak
    public class EmailSettingsController : ControllerBase
    {
        private readonly ApiContext _context;

        public EmailSettingsController(ApiContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME (React Tablosu İçin)
        [HttpGet]
        public async Task<IActionResult> GetUserSettings()
        {
            try
            {
                var list = await _context.Users
                    .Include(u => u.EmailSetting)
                    .Select(u => new UserEmailSettingDto
                    {
                        UserId = u.Id,
                        // İsim yoksa İsimsiz yazsın
                        FullName = u.Name ?? "İsimsiz",
                        Email = u.Email ?? "",

                        // Ayarı varsa getir, yoksa varsayılanı bas
                        IsActive = u.EmailSetting != null ? u.EmailSetting.IsActive : false,
                        Frequency = u.EmailSetting != null ? u.EmailSetting.Frequency : "Weekly",
                        Days = u.EmailSetting != null ? u.EmailSetting.Days : "",
                        ReportType = u.EmailSetting != null ? u.EmailSetting.ReportType : "OnlyStock"
                    })
                    .ToListAsync();

              
                

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Veri çekme hatası: " + ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserSetting([FromBody] UserEmailSettingDto dto)
        {
           
            Console.WriteLine($"GELEN VERİ -> UserID: {dto.UserId}, Active: {dto.IsActive}");

            if (dto.UserId == 0)
            {
                return BadRequest("HATA: Kullanıcı ID'si 0 geldi.");
            }

            try
            {
                var setting = await _context.UserEmailSettings.FirstOrDefaultAsync(x => x.UserId == dto.UserId);

                if (setting == null)
                {
                   
                    var newSetting = new UserEmailSetting
                    {
                        UserId = dto.UserId,
                        IsActive = dto.IsActive,
                        Frequency = string.IsNullOrEmpty(dto.Frequency) ? "Weekly" : dto.Frequency,
                        Days = string.IsNullOrEmpty(dto.Days) ? "1" : dto.Days,
                        ReportType = string.IsNullOrEmpty(dto.ReportType) ? "OnlyStock" : dto.ReportType
                    };
                    _context.UserEmailSettings.Add(newSetting);
                }
                else
                {
                   
                    setting.IsActive = dto.IsActive;

                    if (!string.IsNullOrEmpty(dto.Frequency)) setting.Frequency = dto.Frequency;
                    if (dto.Days != null) setting.Days = dto.Days;
                    if (!string.IsNullOrEmpty(dto.ReportType)) setting.ReportType = dto.ReportType;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Kayıt Başarılı" });
            }
            catch (Exception ex)
            {
                
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, "Kayıt Hatası: " + ex.Message + " | " + innerMsg);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserSettingPut(int userId, [FromBody] UserEmailSettingDto dto)
        {
            Console.WriteLine($"PUT İSTEĞİ GELDI -> UserID: {userId}, Active: {dto.IsActive}");

            if (userId == 0 || dto.UserId != userId)
            {
                return BadRequest("HATA: Kullanıcı ID'si uyuşmuyor.");
            }

            try
            {
                var setting = await _context.UserEmailSettings
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (setting == null)
                {
                    var newSetting = new UserEmailSetting
                    {
                        UserId = userId,
                        IsActive = dto.IsActive,
                        Frequency = string.IsNullOrEmpty(dto.Frequency) ? "Weekly" : dto.Frequency,
                        Days = string.IsNullOrEmpty(dto.Days) ? "1" : dto.Days,
                        ReportType = string.IsNullOrEmpty(dto.ReportType) ? "OnlyStock" : dto.ReportType
                    };

                    _context.UserEmailSettings.Add(newSetting);
                    await _context.SaveChangesAsync();

                   

                    return Ok(new
                    {
                        message = "Ayar oluşturuldu",
                        userId = newSetting.UserId,
                        isActive = newSetting.IsActive,
                        frequency = newSetting.Frequency,
                        reportType = newSetting.ReportType,
                        days = newSetting.Days
                    });
                }
                else
                {
                    //  Önce eski değeri logla
                    Console.WriteLine($" GÜNCELLEME ÖNCESI -> UserID: {userId}, IsActive: {setting.IsActive}");

                    setting.IsActive = dto.IsActive;

                    if (!string.IsNullOrEmpty(dto.Frequency))
                        setting.Frequency = dto.Frequency;

                    if (dto.Days != null)
                        setting.Days = dto.Days;

                    if (!string.IsNullOrEmpty(dto.ReportType))
                        setting.ReportType = dto.ReportType;

                  
                    await _context.SaveChangesAsync();

                    
                    Console.WriteLine($" GÜNCELLEME SONRASI -> UserID: {userId}, IsActive: {setting.IsActive}");

                    return Ok(new
                    {
                        message = "Ayar güncellendi",
                        userId = setting.UserId,
                        isActive = setting.IsActive,
                        frequency = setting.Frequency,
                        reportType = setting.ReportType,
                        days = setting.Days
                    });
                }
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException?.Message ?? "";
                Console.WriteLine($" HATA: {ex.Message} | Inner: {innerMsg}");

                return StatusCode(500, new
                {
                    error = "Kayıt Hatası",
                    message = ex.Message,
                    innerException = innerMsg
                });
            }
        }
    }
}