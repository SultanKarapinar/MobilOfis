using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApiContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Name == username);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }//parola dogrulama 
        public async Task<User> GetByEmailAsync(string email)
        {
            
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges(); // Hem günceller hem kaydeder
        }
    }
}
