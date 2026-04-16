using OfisUrunTakip.WebApi.Entity;
using System.ComponentModel.DataAnnotations;

namespace DTO.ProductDTOs
{
    public class ProductAddDto
    {
        [Required]
        public string Name { get; set; }
       // [Required]
      // public decimal PurchasePrice { get; set; }//alış fiyatı
        [Required]
        public int CategoryId { get; set; }//kategory ıd
       [Required]
        public UnitOfMeasure UnitOfMeasure { get; set; } // ADET KUTU
       // public int CurrentStock { get; set; }//mevcut stok
        public decimal ReorderLevel { get; set; }//min stok seviyesi
      //  [Required]
     //  public int SupplierId { get; set; }//tedarikçi ıd

       // public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
