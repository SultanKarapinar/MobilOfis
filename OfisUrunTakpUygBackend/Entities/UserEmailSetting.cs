using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OfisUrunTakip.WebApi.Entity;

namespace Entities
{
    public class UserEmailSetting
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public bool IsActive { get; set; } //aktıf mı
        public string Frequency { get; set; } //sıklık
        public string Days { get; set; } //hangı gun
        public string ReportType { get; set; } // rapor turu

        [ForeignKey("UserId")] 
        public virtual User User { get; set; }
    }
}