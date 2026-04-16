using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class EmailNotificationRepository
        : GenericRepository<EmailNotification>, IEmailNotificationRepository
    {
        public EmailNotificationRepository(ApiContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<EmailNotification>> GetAllWithUserAsync()
        {
            return await _context.EmailNotifications
                .Include(x => x.User)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();
        }
    }
}
