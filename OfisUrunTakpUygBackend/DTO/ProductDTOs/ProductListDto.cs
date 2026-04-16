using OfisUrunTakip.WebApi.Entity;

namespace DTO.ProductDTOs
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }//alış fiyatı
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }//kategory ıd
        public UnitOfMeasure UnitOfMeasure { get; set; } // ADET KUTU
        public decimal CurrentStock { get; set; }//mevcut stok
        public decimal ReorderLevel { get; set; }//min stok seviyesi
       // public int SupplierId { get; set; }//tedarikçi ıd

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
