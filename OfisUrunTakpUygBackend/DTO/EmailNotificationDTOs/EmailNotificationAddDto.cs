using System.ComponentModel.DataAnnotations;
using OfisUrunTakip.WebApi.Entity;

namespace DTO.EmailNotificationDTOs
{
    public class EmailNotificationAddDto
    {
        public bool Enabled { get; set; }
        public string TargetRole { get; set; }
        public int Frequency { get; set; }

     
        public int? UserId { get; set; }
        public DateTime SentDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pending;
        public string? Message { get; set; }
    }
}