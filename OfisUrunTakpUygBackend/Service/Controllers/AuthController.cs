using DTO.LoginDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;//appsettingden ayarları okumak için


        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _config = config;
            _userRepository = userRepository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                return Unauthorized("kullanıcı bulunamadı");
            }
            if (user.Password == null || user.Password != dto.Password)
            {
                return Unauthorized("Şifre yanlış");
                //Unauthorized gerkli kimlik dogrulamasına sahıp degılsınız

            }

            var token = JwtToken(user);
            return Ok(new { token });
        }


        private string JwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //sifreyi algoritmaya gore sifreler

            var userClaims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim (ClaimTypes.Name,user.Name),
            new Claim(ClaimTypes.Role,user.Role),
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(6), //biz burada 6 ekle dedık
                                                      //ama turkıye utc+3 kullandıgı için 
                                                      //3 saat kullanmasını ister buna dıkkat e 
                signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
