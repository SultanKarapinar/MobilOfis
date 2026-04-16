using OfisUrunTakip.WebApi.Entity;

namespace DTO.StockTransactionDTOs
{
    public class StockTransactionAddDto
    {


        public int ProductId { get; set; }//urunıd
        public TransactionType TransactionType { get; set; }//işlem turu giriş cıkıs
        public decimal Quantity { get; set; }// giren cıkan urun adedi
        public decimal UnitPrice { get; set; }//alış satış fiaları
      //  public DateTime TransactionDate { get; set; } = DateTime.Now;
            //işlemin gerçeklestiği tarih
        public int UserId { get; set; }//işlemi yapan kısı
        public string? Description { get; set; }//acıklama
      public decimal Totalcons { get; set; } // toplam harcama o urun için
        public int? SupplierId { get; set; }
    }
    //public enum TransactionType
    //{
    //    Out = -1,
    //    In = 1
    //}
}
