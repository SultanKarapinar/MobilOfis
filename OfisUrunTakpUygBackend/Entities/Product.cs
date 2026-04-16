using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class Product : EntityBase
    {

        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }//alış fiyatı

        public int CategoryId { get; set; }//kategory ıd
        public UnitOfMeasure UnitOfMeasure { get; set; } //birim turu
        public decimal CurrentStock { get; set; } = 0;//mevcut stok
        public decimal ReorderLevel { get; set; }//min stok seviyesi
        public int? SupplierId { get; set; }//tedarikçi ıd

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false; //soft delete yanı yumusak silme sılınse bıle tutulur 

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
        public ICollection<EmailNotification> EmailNotifications { get; }

    }
    public enum  UnitOfMeasure
    {
        Kg = 0,
        Paket= 1,
        Litre=2,
        Adet=3
    }

}