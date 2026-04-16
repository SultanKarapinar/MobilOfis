using AutoMapper;
using DTO.EmailNotificationDTOs;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailNotificationsController : ControllerBase
    {
        private readonly IEmailNotificationSettingRepository _settingRepository; // Ayarlar için
        private readonly IEmailNotificationRepository _emailRepository;         // Loglar (Gönderilenler) için
        private readonly IMapper _mapper;

      
        public EmailNotificationsController(IMapper mapper,
            IEmailNotificationSettingRepository settingRepository,
            IEmailNotificationRepository emailRepository)
        {
            _settingRepository = settingRepository;
            _emailRepository = emailRepository;
            _mapper = mapper;
        }

       

        [HttpGet("settings")] // Adresi özelleştirdik karışmasın diye
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _settingRepository.GetAllAsync();
            return Ok(settings);
        }

        [HttpPost("settings")]
        public async Task<IActionResult> CreateSettings([FromBody] EmailNotificationAddDto dto)
        {
            if (dto == null) return BadRequest();
            var setting = _mapper.Map<EmailNotificationSetting>(dto);
            setting.UpdatedAt = DateTime.Now;
            await _settingRepository.AddAsync(setting);
            return Ok(setting);
        }

        [HttpPut("settings/{id}")]
        public async Task<IActionResult> UpdateSettings(int id, [FromBody] EmailNotificationUpdateDto dto)
        {
            var setting = await _settingRepository.GetByIdAsync(id);
            if (setting == null) return NotFound();
            _mapper.Map(dto, setting);
            setting.UpdatedAt = DateTime.Now;
            await _settingRepository.UpdateAsync(setting);
            return Ok(setting);
        }

        // --- LOGLAR KISMI ---

    
        [HttpGet]
        public async Task<IActionResult> GetSentEmails()
        {
            // Burada artık Log Repository kullanıyoruz
            var emails = await _emailRepository.GetAllWithUserAsync();
            var emailDto = _mapper.Map<IEnumerable<EmailNotificationListDto>>(emails);
            return Ok(emailDto);
        }

       

       
    }
}