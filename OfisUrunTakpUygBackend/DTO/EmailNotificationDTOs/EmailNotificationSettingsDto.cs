namespace OfisUrunTakip.WebApi.DTO // Namespace senin projene göre farklı olabilir, dikkat et
{
    public class UserEmailSettingDto
    {
        // Soru işareti (?) ekleyerek bu alanların boş gelmesine izin veriyoruz.
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string? Frequency { get; set; }
        public string? Days { get; set; }
        public string? ReportType { get; set; }
    }
}