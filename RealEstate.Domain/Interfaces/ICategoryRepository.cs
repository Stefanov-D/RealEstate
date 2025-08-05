using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category, Guid>, IAsyncGenericRepository<Category, Guid>
    {
    }
}
