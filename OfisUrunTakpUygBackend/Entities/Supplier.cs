using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class Supplier : EntityBase
    {//tedarikçiler
        public string Name { get; set; }//isni
        public int Id { get; set; }
        public string Phone { get; set; }//telefon
        public string Email { get; set; }//mail
        public string Address { get; set; }//adres
        public string TaxNumber { get; set; }  //vergi no 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        //tarih
        public ICollection<Product> Products { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
