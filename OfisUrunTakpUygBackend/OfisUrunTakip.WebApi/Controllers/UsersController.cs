using AutoMapper;
using DTO.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserRepository userRepository, IMapper mapper, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Tüm kullanıcılar listeleniyor.");
            var data = await _userRepository.GetAllAsync();
            var d = _mapper.Map<IEnumerable<User>>(data);
            _logger.LogInformation("Toplam {Count} kullanıcı getirildi.", d.Count());
            return Ok(d);
        }

        [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] UserAddDto dto)
        {
            _logger.LogInformation("Yeni kullanıcı ekleme isteği alındı. Email: {Email}", dto.Email);

            if (await _userRepository.ExistsAsync(x => x.Email == dto.Email))
            {
                _logger.LogWarning("Kullanıcı eklenemedi, zaten mevcut. Email: {Email}", dto.Email);
                return BadRequest("Bu kullanıcı zaten var!");
            }

            if (dto == null)
            {
                _logger.LogWarning("Kullanıcı ekleme isteği boş DTO ile geldi.");
                return BadRequest();
            }

            var user = _mapper.Map<User>(dto);

            user.Password = _userRepository.HashPassword(dto.Password);

            await _userRepository.AddAsync(user);

            _logger.LogInformation("Yeni kullanıcı eklendi. Id: {Id}, Email: {Email}", user.Id, user.Email);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Kullanıcı silme isteği alındı. Id: {Id}", id);
            var data = await _userRepository.RemoveAsync(id);
            if (data == null)
            {
                _logger.LogWarning("Silinmek istenen kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Kullanıcı silindi. Id: {Id}", id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            _logger.LogInformation("Kullanıcı güncelleme isteği alındı. Id: {Id}", id);

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("Güncellenmek istenen kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound();
            }

            var oldPasswordHash = user.Password;

            _mapper.Map(dto, user);

            if (string.IsNullOrEmpty(dto.Password))
            {
                user.Password = oldPasswordHash;
                _logger.LogInformation("Şifre güncellenmedi. Id: {Id}", id);
            }
            else
            {
                user.Password = _userRepository.HashPassword(dto.Password);
                _logger.LogInformation("Şifre güncellendi. Id: {Id}", id);
            }

            var updateUser = await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Kullanıcı güncellendi. Id: {Id}", id);

            return Ok(updateUser);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Id ile kullanıcı bilgisi getiriliyor. Id: {Id}", id);
            var a = await _userRepository.GetByIdAsync(id);
            if (a == null)
            {
                _logger.LogWarning("Id ile kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound();
            }
            return Ok(a);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto dto)
        {
            _logger.LogInformation("Profil güncelleme isteği alındı. Kullanıcı Id: {Id}", dto.Id);

            var user = await _userRepository.GetByIdAsync(dto.Id);

            if (user == null)
            {
                _logger.LogWarning("Profil güncellenmek istenen kullanıcı bulunamadı. Id: {Id}", dto.Id);
                return NotFound("Kullanıcı bulunamadı.");
            }

            user.Name = dto.Name;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                _logger.LogInformation("Şifre değiştirme isteği mevcut. Kullanıcı Id: {Id}", dto.Id);

                if (string.IsNullOrEmpty(dto.OldPassword))
                {
                    _logger.LogWarning("Şifre değiştirme başarısız: eski şifre girilmedi. Kullanıcı Id: {Id}", dto.Id);
                    return BadRequest("Şifrenizi değiştirmek için mevcut (eski) şifrenizi girmelisiniz.");
                }

                if (!_userRepository.VerifyPassword(dto.OldPassword, user.Password))
                {
                    _logger.LogWarning("Şifre değiştirme başarısız: eski şifre hatalı. Kullanıcı Id: {Id}", dto.Id);
                    return BadRequest("Girdiğiniz mevcut şifre hatalı.");
                }

                user.Password = _userRepository.HashPassword(dto.NewPassword);
                _logger.LogInformation("Şifre başarıyla değiştirildi. Kullanıcı Id: {Id}", dto.Id);
            }

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Profil başarıyla güncellendi. Kullanıcı Id: {Id}", dto.Id);

            return Ok(user);
        }

        public class ProfileUpdateDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string? OldPassword { get; set; }
            public string? NewPassword { get; set; }
        }
    }
}
