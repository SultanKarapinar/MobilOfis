using System.ComponentModel.DataAnnotations;

namespace DTO.UserDTOs
{
    public class UserAddDto
    {
        [Required]
        public string Name { get; set; }//kulanıcı ismi
        [Required]
        public string Role { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
       // public DateTime CreatedDate { get; set; }
      //  public DateTime LastLoginDate { get; set; }
    }
}
