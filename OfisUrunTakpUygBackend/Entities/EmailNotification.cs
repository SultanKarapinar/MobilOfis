using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class EmailNotification : EntityBase
    {


        public int? UserId { get; set; }//bildirimi olan kullancı
        public DateTime SentDate { get; set; } = DateTime.Now;
        //gönderim tarihi
        public Status Status { get; set; }
        public string Message { get; set; }
        public User User { get; set; }


        //productıd ye gerek yok cunku her product ıcın mail gönderme
        //icin her hatfa tarama yapacak
    }

    public enum Status //sabit deger kullandıgında enum ve degerlerı verdın
    {
        Pending,
        Sent,
        Failed
    }
}
