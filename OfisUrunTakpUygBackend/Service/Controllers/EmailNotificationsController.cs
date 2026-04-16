using AutoMapper;
using DTO.EmailNotificationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailNotificationsController : ControllerBase
    {
        private readonly IEmailNotificationRepository _emailRepository;
        private readonly IMapper _mapper;

        public EmailNotificationsController(IMapper mapper, IEmailNotificationRepository EmailNotificationRepository)

        {
            _emailRepository = EmailNotificationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var emails = await _emailRepository.GetAllAsync();
            var emailDto = _mapper.Map<IEnumerable<EmailNotificationListDto>>(emails);
            return Ok(emailDto);
        }

        [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] EmailNotificationAddDto dto)

        {
            if (dto == null)
            {
                return BadRequest();
            }
            var emails = _mapper.Map<EmailNotification>(dto);
            await _emailRepository.AddAsync(emails);

            return Ok(emails);
        }
        [HttpDelete]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _emailRepository.RemoveAsync(id);
            if (data == null)
            { return NotFound(); }
            return Ok(data);

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, EmailNotificationUpdateDto dto)

        {

            var email = await _emailRepository.GetByIdAsync(id);
            if (email == null) { return NotFound(); }
            _mapper.Map(dto, email);
            var emailUpdate = await _emailRepository.UpdateAsync(email);
            return Ok(emailUpdate);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var email = await _emailRepository.GetByIdAsync(id);
            if (email == null)
            { return BadRequest(); }
            return Ok(email);
        }


    }
}
