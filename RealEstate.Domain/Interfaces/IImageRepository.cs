using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IImageRepository : IGenericRepository<Image, Guid>, IAsyncGenericRepository<Image, Guid>
    {
    }
}
