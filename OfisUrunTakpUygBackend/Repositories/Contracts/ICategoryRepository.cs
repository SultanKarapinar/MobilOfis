using OfisUrunTakip.WebApi.Entity;

namespace Repositories.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        List<Product> GetProductsByCategoryId(int categoryId);
        Task<bool> HasProductsAsync(int categoryId);
    }
}
