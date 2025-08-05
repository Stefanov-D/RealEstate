using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>, IAsyncGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly ApplicationDbContext DbContext;
        private readonly DbSet<TEntity> DbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        public TEntity? GetById(TKey id) => DbSet.Find(id);

        public async Task<TEntity?> GetByIdAsync(TKey id) => await DbSet.FindAsync(id);

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate) => DbSet.FirstOrDefault(predicate);

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate) => DbSet.SingleOrDefault(predicate);

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.SingleOrDefaultAsync(predicate);

        public IEnumerable<TEntity?> GetAll() => DbSet.ToList();

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await DbSet.ToListAsync();

        public IQueryable<TEntity> GetAllAttached() => DbSet;

        public void Add(TEntity item) => DbSet.Add(item);

        public async Task AddAsync(TEntity item) => await DbSet.AddAsync(item);

        public void AddRange(IEnumerable<TEntity> items) => DbSet.AddRange(items);

        public async Task AddRangeAsync(IEnumerable<TEntity> items) => await DbSet.AddRangeAsync(items);

        public bool Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            return true;
        }

        public bool SoftDelete(TEntity entity)
        {
            var prop = typeof(TEntity).GetProperty("IsDeleted");
            if (prop != null)
            {
                prop.SetValue(entity, true);
                DbContext.Entry(entity).State = EntityState.Modified;
                return true;
            }
            return false;
        }

        public async Task<bool> SoftDeleteAsync(TEntity entity)
        {
            var success = SoftDelete(entity);
            if (success)
            {
                await SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Update(TEntity item)
        {
            DbSet.Update(item);
            return true;
        }

        public bool UpdateAsync(TEntity item)
        {
            DbSet.Update(item);
            return true;
        }

        public async Task<TEntity?> FindByConditionsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public void SaveChanges() => DbContext.SaveChanges();

        public async Task SaveChangesAsync() => await DbContext.SaveChangesAsync();
    }
}
