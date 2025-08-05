using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IListingTypeRepository : IGenericRepository<ListingType, Guid>, IAsyncGenericRepository<ListingType, Guid>
    {
    }
}
