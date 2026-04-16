using System.ComponentModel.DataAnnotations;

namespace DTO.EmailNotificationDTOs
{
    public class EmailNotificationUpdateDto
    {
        // React'ten gelen verilerle birebir eşleşmeli:
        public bool Enabled { get; set; }        
        public string TargetRole { get; set; }   
        public int Frequency { get; set; }       

       
        public int? UserId { get; set; }
        public string? Message { get; set; }
    }
}
