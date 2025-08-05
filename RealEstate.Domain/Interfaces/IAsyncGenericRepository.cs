using System.Linq.Expressions;

namespace RealEstate.Domain.Interfaces
{
    public interface IAsyncGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity item);
        Task AddRangeAsync(IEnumerable<TEntity> items);
        Task<bool> SoftDeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity?> FindByConditionsAsync(Expression<Func<TEntity, bool>> predicate);
        Task SaveChangesAsync();
    }
}
