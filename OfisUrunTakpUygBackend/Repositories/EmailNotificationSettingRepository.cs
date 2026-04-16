using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class EmailNotificationSettingRepository : GenericRepository<EmailNotificationSetting>, IEmailNotificationSettingRepository
    {
        public EmailNotificationSettingRepository(ApiContext context) : base(context)
        {
        }
    }
}