using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class StockTransaction : EntityBase
    {

        public int ProductId { get; set; }//urunıd
       
        public TransactionType TransactionType { get; set; }//işlem turu giriş cıkıs -1,1
        public decimal Quantity { get; set; }// giren cıkan urun adedi
        public decimal UnitPrice { get; set; }//alış satış fiaları
        public DateTime TransactionDate { get; set; } = DateTime.Now; //işlemin gerçeklestiği tarih
        public int UserId { get; set; }//işlemi yapan kısı
        public int? SupplierId { get; set; }
        public string? Description { get; set; }//acıklama
        public decimal Totalcons { get; set; }// toplam  harcanan mıktar
        public Product Product { get; set; }
        public Supplier? Supplier { get; set; }
        public User User { get; set; }



    }
    public enum TransactionType
    {
        Out = -1,
        In = 1
    }
}
