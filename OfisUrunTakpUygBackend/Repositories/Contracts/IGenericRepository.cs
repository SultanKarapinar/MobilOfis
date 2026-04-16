using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T t);
        Task<T> UpdateAsync(T t);
        Task<T> RemoveAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(params string[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
// burada eger biz generic sınıfını olusturmazsak
// tüm intefacelerin içine gidip tek tek bu metodları
// tannımlamak zorundaydık ve bu da yapıya sıkıntı verır
// o yuzden bız bır kere tanımlayalım sonrada diger
// sınıflar bu sınıftan kalıtım alsın diye düşündük 
// task kullanarak asnkoreon sistem kullandık
// ve list yerine IEnumerable kullandık
// daha genel olsun diye
