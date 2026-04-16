using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class EmailNotificationSetting : EntityBase
    {
        public bool Enabled { get; set; } = false;

        // User.Role string olduğu için bu da string
        public string TargetRole { get; set; } = "Asistan";

        public MailFrequency Frequency { get; set; } = MailFrequency.Daily;

        public DateTime? LastSentAt { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public enum MailFrequency
    {
        Daily = 1,
        Every2Days = 2,
        Weekly = 3,
        Monthly = 4
    }
}
