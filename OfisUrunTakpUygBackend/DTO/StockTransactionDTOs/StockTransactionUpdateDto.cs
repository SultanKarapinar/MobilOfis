namespace DTO.StockTransactionDTOs
{
    public class StockTransactionUpdateDto
    {
        public int ProductId { get; set; }//urunıd
        public string TransactionType { get; set; }//işlem turu giriş cıkıs
        public decimal Quantity { get; set; }// giren cıkan urun adedi
        public decimal UnitPrice { get; set; }//alış satış fiaları
        public DateTime TransactionDate { get; set; } //işlemin gerçeklestiği tarih
        public int UserId { get; set; }//işlemi yapan kısı
        public string? Description { get; set; }//acıklama
        public int? SupplierId { get; set; }
    }
}
