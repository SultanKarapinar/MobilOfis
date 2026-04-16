using OfisUrunTakip.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;

namespace OfisUrunTakip.WebApi.Services
{
    public class EmailReportService
    {
        private readonly ApiContext _context;
        private readonly IEmailSender _emailSender;

        public EmailReportService(ApiContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task ProcessEmails()
        {
            var today = DateTime.Now;//bu gun bılgısı
            var dayOfWeek = (int)today.DayOfWeek;//0-6 arasında deger 
            var dayOfMonth = today.Day;//ayın kacuncı gunu

            var activeUsers = await _context.UserEmailSettings//aktıf kullanıcılarıcejıyor 
                .Include(x => x.User)
                .Where(x => x.IsActive)
                .ToListAsync();

            foreach (var setting in activeUsers)//her kullanıcı için kontrol
            {
                bool shouldSend = false;//ilk gonderılmeyecek sekılde ayalandıı

                var days = setting.Days.Split(',');

                if (setting.Frequency == "Weekly" &&
                    days.Contains(dayOfWeek.ToString()))
                {
                    shouldSend = true;
                }//jkonrol edıyor bu gun lıstede varsa gonderır 
                else if (setting.Frequency == "Monthly" &&
                         setting.Days == dayOfMonth.ToString())
                {
                    shouldSend = true;
                }

                if (shouldSend)//rapor çesidi bulunur 
                {
                    string emailBody = setting.ReportType == "FullReport"
                        ? GenerateFullMonthlyReport()
                        : GenerateStockOnlyReport();

                    await _emailSender.SendAsync(
                        setting.User.Email,
                        "Bilgilendirme Raporu",
                        emailBody,
                        CancellationToken.None
                    );
                }
            }
        }

        private string GenerateFullMonthlyReport()
        {
            return "Full Report Content";
        }

        private string GenerateStockOnlyReport()
        {
            return "Stock Only Report Content";
        }
    }
}
