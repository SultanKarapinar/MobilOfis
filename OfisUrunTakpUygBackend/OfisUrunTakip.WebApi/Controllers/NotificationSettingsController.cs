/* using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.DTO;
using OfisUrunTakip.WebApi.Entity;

namespace OfisUrunTakip.WebApi.Controllers
{
    [ApiController]
    [Route("api/notification-settings")]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly ApiContext _db;

        public NotificationSettingsController(ApiContext db)
        {
            _db = db;
        }

        [HttpGet("email")]
        public async Task<ActionResult<EmailNotificationSettingsDto>> GetEmailSettings()
        {
            var s = await _db.EmailNotificationSettings.FirstOrDefaultAsync();

            if (s == null)
            {
                return Ok(new EmailNotificationSettingsDto
                {
                    Enabled = false,
                    TargetRole = "Asistan",
                    Frequency = MailFrequency.Daily
                });
            }

            return Ok(new EmailNotificationSettingsDto
            {
                Enabled = s.Enabled,
                TargetRole = s.TargetRole,
                Frequency = s.Frequency
            });
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmailSettings([FromBody] EmailNotificationSettingsDto dto)
        {
            var s = await _db.EmailNotificationSettings.FirstOrDefaultAsync();

            if (s == null)
            {
                s = new EmailNotificationSetting();
                _db.EmailNotificationSettings.Add(s);
            }

            s.Enabled = dto.Enabled;
            s.TargetRole = string.IsNullOrWhiteSpace(dto.TargetRole)
    ? "Asistan"
    : dto.TargetRole;

            s.Frequency = dto.Frequency;
            s.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
} */
