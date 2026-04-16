using System.ComponentModel.DataAnnotations;

namespace DTO.SupplierDTOs
{
    public class SupplierUpdateDto
    {

        public string Name { get; set; }//isni
       // public string ProductType { get; set; }//ne satıyor
        public string TaxNumber { get; set; }  //vergi no 
        public string Phone { get; set; }//telefon
        [Required]
        public string Email { get; set; }//mail
        public string Address { get; set; }//adres
       // public DateTime CreatedDate { get; set; }//tarih
    }
}
