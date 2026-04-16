using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;
// Eğer EntityBase farklı bir yerdeyse buraya 'using Entities;' eklemen gerekebilir, 
// ama genelde aynı namespace altındadır.

namespace OfisUrunTakip.WebApi.Entity
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiration { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate { get; set; } = DateTime.Now;

        // Mevcut İlişkilerin
        public ICollection<EmailNotification> EmailNotifications { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }

        // --- YENİ EKLENEN SATIR ---
        public virtual UserEmailSetting EmailSetting { get; set; }

    } // User class bitişi
} // Namespace bitişi