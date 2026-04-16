using System.ComponentModel.DataAnnotations;

namespace DTO.SupplierDTOs
{
    public class SupplierAddDto
    {
        [Required]
        public string Name { get; set; }//isni

        public string TaxNumber { get; set; }  //vergi no 
        [Phone]
        public string Phone { get; set; }//telefon
        [EmailAddress]
        public string Email { get; set; }//mail
        public string Address { get; set; }//adres
        public DateTime CreatedDate { get; set; }//tarih
    }
}
