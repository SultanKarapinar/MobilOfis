using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApiContext context) : base(context)
        {
        }
    }
}
