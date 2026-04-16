using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Repositories

{  // sen IGeneric<T>d en bir kalıtım al ve bu T sınıf olmalı dedim
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly ApiContext _context;
        public GenericRepository(ApiContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            await _context.SaveChangesAsync();
            //burada neden return yapmadık 
            //çünkü IGeneric tanımlarken biz Task diye tanımladık
            //ve bu bize task döndurur
            // ama return deseydık bize Task<EntityEntry <T> dödurur 
            //buda tür uyuşmazlıgı olur 
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
            //sorgula ekledıgım öge var mı bak
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

           
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            //  Product ise IsDeleted = false filtrele
            if (typeof(T) == typeof(Product))
            {
                query = query.Cast<Product>()   
                             .Where(p => !p.IsDeleted)
                             .Cast<T>();      
            }

            return await query.ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
            {
            return await _context.Set<T>().FindAsync(id);

            // _context.Set<t() Ef coreden geliyor tabloyu getir diyor 
            //.FindAsync(id) EF coreden geliyor tablodan belirtilen id ye sahip kaydı bul 
        }

        public async Task<T> RemoveAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
                return null;

            // Product için soft delete 
            if (entity is Product product)
            {
                // Stok işlemi var mı 
                var hasStockTransactions = await _context.Set<StockTransaction>()
                    .AnyAsync(s => s.ProductId == product.Id);

                if (hasStockTransactions)
                {
                    // soft delete
                    product.IsDeleted = true;
                }
                else
                {
                    // stok yoksa 
                    _context.Set<T>().Remove(entity);
                }
            }
            else
            {
                // Diğer entity’ler normal silinsin
                _context.Set<T>().Remove(entity);
            }

           
            await _context.SaveChangesAsync();
            return entity;
        }



        public async Task<T> UpdateAsync(T t)
        {


            _context.Set<T>().Update(t);
            await _context.SaveChangesAsync();
            return t;
        }
    }
}
