namespace DTO.UserDTOs
{
    public class UserListDto
    {

        public int Id { get; set; }//kullanıcı ıd
        public string Name { get; set; }//kulanıcı ismi
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
