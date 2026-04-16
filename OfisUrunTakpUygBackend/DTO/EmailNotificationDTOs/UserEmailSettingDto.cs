using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfisUrunTakip.DTO.EmailNotificationDTOs
{
    public class UserEmailSettingDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } // To display the name in the table 
        public string Email { get; set; }    // To display the mail in the table 
        public bool IsActive { get; set; }   // for the switch button 
        public string Frequency { get; set; } // "Weekly", "Monthly"
        public string Days { get; set; }      // "Pazartesi", "3" gibi değerler
        public string ReportType { get; set; } // "OnlyStock" veya "FullReport"
    }
}