using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApiContext context) : base(context)
        {
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products
           .Where(p => p.CategoryId == categoryId)
           .ToList();
        }
        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _context.Products.AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted);
        }
    }
}
