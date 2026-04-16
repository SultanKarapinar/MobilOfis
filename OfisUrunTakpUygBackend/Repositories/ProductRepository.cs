using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApiContext context) : base(context)
        {
        }
    }
}
