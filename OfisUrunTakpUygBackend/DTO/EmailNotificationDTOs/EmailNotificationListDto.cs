using OfisUrunTakip.WebApi.Entity;

namespace DTO.EmailNotificationDTOs
{
    public class EmailNotificationListDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }

        public DateTime SentDate { get; set; }
        public Status Status { get; set; }
        public string Message { get; set; } = "";
    }
}
