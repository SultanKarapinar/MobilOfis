namespace DTO.SupplierDTOs
{
    public class SupplierListDto
    {
        public int Id { get; set; }//ıd
        public string Name { get; set; }//isni

        public string TaxNumber { get; set; }  //vergi no 
        public string Phone { get; set; }//telefon
        public string Email { get; set; }//mail
        public string Address { get; set; }//adres
        public DateTime CreatedDate { get; set; }//tarih
    }
}
