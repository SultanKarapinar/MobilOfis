using OfisUrunTakip.WebApi.Entity;

namespace Repositories.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        string HashPassword(string password);//hashleme yaptık
        bool VerifyPassword(string password, string hashedPassword);//dogrulama
        Task<User> GetByEmailAsync(string email);
        void Update(User user);

    }
}

