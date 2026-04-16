using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OfisUrunTakip.WebApi.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)//appsettın ıcındekı ayarlat
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
        {
            var host = _config["Smtp:Host"];//smtpsunucu adres
            var port = int.Parse(_config["Smtp:Port"] ?? "587");
            var enableSsl = bool.Parse(_config["Smtp:EnableSsl"] ?? "true");//guvenlık
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];
            var from = _config["Smtp:From"] ?? username;//gonderen mail adrsi
            var fromName = _config["Smtp:FromName"] ?? "Merkez Ofisim";//gorunen ısım

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("SMTP ayarları eksik. appsettings.json içindeki Smtp bölümünü kontrol et.");

            using var message = new MailMessage();//mail nesnesi
            message.From = new MailAddress(from!, fromName);
            message.To.Add(toEmail);//alıcı
            message.Subject = subject;//baslık
            message.Body = htmlBody;//içrik
            message.IsBodyHtml = true;//html olarak 

            using var client = new SmtpClient(host, port);
            client.EnableSsl = enableSsl;
            client.Credentials = new NetworkCredential(username, password);

            await client.SendMailAsync(message);//mail gonder
        }
    }
}
