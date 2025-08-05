namespace RealEstate.Domain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        TEntity? GetById(TKey id);
        TEntity? FirstOrDefault(Func<TEntity, bool> predicate);
        TEntity? SingleOrDefault(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAllAttached();
        void Add(TEntity item);
        void AddRange(IEnumerable<TEntity> items);
        bool SoftDelete(TEntity entity);
        bool Delete(TEntity entity);
        bool Update(TEntity item);
        void SaveChanges();
    }
}
