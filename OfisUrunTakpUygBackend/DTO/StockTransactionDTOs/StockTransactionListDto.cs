namespace DTO.StockTransactionDTOs
{
    public class StockTransactionListDto
    {

        public int Id { get; set; }//ıd
        public int ProductId { get; set; }//urunıd
        public string ProductName { get; set; }
        public string TransactionType { get; set; }//işlem turu giriş cıkıs
        public decimal Quantity { get; set; }// giren cıkan urun adedi
        public decimal UnitPrice { get; set; }//alış satış fiaları
        public DateTime TransactionDate { get; set; } //işlemin gerçeklestiği tarih
        public int UserId { get; set; }//işlemi yapan kısı
        public string UserName { get; set; }
        public string? Description { get; set; }//acıklama
        public decimal Totalcons { get; set; } // toplam harcama o urun için
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}

