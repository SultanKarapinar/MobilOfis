using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace Repositories
{
    public class StockTransactionRepository : GenericRepository<StockTransaction>, IStockTransactionRepository
    {
        public StockTransactionRepository(ApiContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockTransaction>> GetByProductIdAsync(int productId, params string[] includes)
        {
            IQueryable<StockTransaction> query = _context.StockTransactions;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                .Where(t => t.ProductId == productId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}
