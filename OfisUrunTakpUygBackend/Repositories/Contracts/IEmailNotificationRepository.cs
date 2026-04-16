using OfisUrunTakip.WebApi.Entity;

namespace Repositories.Contracts
{
    public interface IEmailNotificationRepository
        : IGenericRepository<EmailNotification>
    {
        Task<IEnumerable<EmailNotification>> GetAllWithUserAsync();
    }
}
