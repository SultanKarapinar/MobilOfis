using DTO.LoginDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;//appsettıngs okuma
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            _logger.LogInformation($"Giriş Denemesi: {dto.Username} sisteme girmeye çalışıyor...");

            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                _logger.LogWarning($"Başarısız Giriş: {dto.Username} kullanıcısı bulunamadı.");
                return Unauthorized("Kullanıcı bulunamadı");
            }

            if (!_userRepository.VerifyPassword(dto.Password, user.Password))//kullanıcı gırdısı ve db dekı hash aynı mı
            {
                _logger.LogWarning($"Hatalı Şifre: {dto.Username} yanlış şifre girdi.");
                return Unauthorized("Şifre yanlış");
            }

            _logger.LogInformation($"Başarılı Giriş: {dto.Username} sisteme giriş yaptı.");

            var token = JwtToken(user);//basarılıysa tooken olsun
            return Ok(new { token });
        }

       
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            _logger.LogInformation($"Şifremi Unuttum İsteği: {email}");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"Şifre Sıfırlama Hatası: {email} adresi sistemde yok.");
                return NotFound("Bu e-posta adresi ile kayıtlı kullanıcı bulunamadı.");
            }

            
            Random rnd = new Random();
            string code = rnd.Next(100000, 999999).ToString();//6 hanelı kod uet rondom
            user.ResetCode = code;
            user.ResetCodeExpiration = DateTime.Now.AddMinutes(3);//kod 3 dk gecerlı olsun
            _userRepository.Update(user);

           
            try
            {
                MailGonder(user.Email, code);
                _logger.LogInformation($"Kod Gönderildi: {email} adresine kod atıldı.");
                return Ok(new { message = "Doğrulama kodu e-posta adresinize gönderildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Mail Gönderme Hatası: {email} adresine mail atılamadı.");
                return BadRequest("Mail gönderilemedi: " + ex.Message);
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)//şifreler ayı mı
            {
                return BadRequest("Girdiğiniz şifreler birbiriyle uyuşmuyor.");
            }

            var user = await _userRepository.GetByEmailAsync(dto.Email);

           
            if (user == null || user.ResetCode != dto.Code)
            {
                _logger.LogWarning($"Şifre Yenileme Başarısız: {dto.Email} kodu hatalı girdi.");
                return BadRequest("Girilen kod hatalı veya kullanıcı bulunamadı.");
            }

          
            if (!user.ResetCodeExpiration.HasValue || user.ResetCodeExpiration.Value < DateTime.Now)//süre kontrolu
            {
                return BadRequest("Bu kodun geçerlilik süresi dolmuş veya geçersiz.");
            }

            
            user.Password = _userRepository.HashPassword(dto.NewPassword);//yenı sıfre hashle

          
            user.ResetCode = null;
            user.ResetCodeExpiration = null;//tekrar kullanılımaz 

            _userRepository.Update(user);

            _logger.LogInformation($"Şifre Değiştirildi: {dto.Email} şifresini güncelledi.");
            return Ok(new { message = "Şifreniz başarıyla güncellendi. Giriş yapabilirsiniz." });
        }

        private void MailGonder(string aliciEmail, string kod)
        {
            string gonderenEmail = _config["MailSettings:Email"];
            string gonderenSifre = _config["MailSettings:Password"];

            if (string.IsNullOrEmpty(gonderenEmail) || string.IsNullOrEmpty(gonderenSifre))
            {
                throw new Exception("Mail ayarları bulunamadı! User Secrets kontrol edilmeli.");
            }

            SmtpClient sc = new SmtpClient();//Smtp baglantısı olustur
            sc.Port = 587;
            sc.Host = "smtp.gmail.com";
            sc.EnableSsl = true;//gmail smtp ayarları
            sc.Credentials = new NetworkCredential(gonderenEmail, gonderenSifre);//gmail logın 

            MailMessage mail = new MailMessage();// mail içeriği
            mail.From = new MailAddress(gonderenEmail, "Merkez Ofisim Güvenlik");
            mail.To.Add(aliciEmail);
            mail.Subject = "Şifre Sıfırlama Kodu";
            mail.Body = $"Merkez Ofisim şifre sıfırlama kodunuz: {kod}";

            sc.Send(mail);//mail gonderılır 
        }

        private string JwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));//token uretır
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]//tokenın ıcıne sunları koy
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],//kim üreti
                audience: _config["Jwt:Audience"],//kim kullanacak
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(3),//3 aatgecerlı
                signingCredentials: credential//imza
            );

            return new JwtSecurityTokenHandler().WriteToken(token);//tokenı strınge cevır
        }
    }
}