using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IListingRepository : IGenericRepository<Listing, Guid>, IAsyncGenericRepository<Listing, Guid>
    {
        Task<IEnumerable<Listing>> GetAllForSaleAsync();
        Task<IEnumerable<Listing>> GetAllForRentAsync();
        Task<IEnumerable<Listing>> GetAllNewEnquiriesAsync();
        Task<Listing?> GetListingWithDetailsAsync(Guid id);
        Task<bool> SyncImageRecordsInDb(Guid listingId, List<string> resolvedImagePaths);
    }
}
