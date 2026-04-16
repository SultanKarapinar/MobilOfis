using OfisUrunTakip.WebApi.Entity;

namespace Repositories.Contracts
{
    public interface IStockTransactionRepository : IGenericRepository<StockTransaction>
    {
        Task<IEnumerable<StockTransaction>> GetByProductIdAsync(int productId, params string[] includes);
    }
}
