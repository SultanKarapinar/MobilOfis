namespace DTO.UserDTOs
{
    public class UserUpdateDto
    {

        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
       
    }
}
