using RealEstate.Application.DTOs;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IListingDataAccess
    {
        public Task<Listing?> GetListingEntityByIdAsync(Guid id);
        public Task<IEnumerable<ListingDto>> GetAllListingsAsync();
        public Task<bool> CreateListingEntityAsync(ListingDto listing);
        public Task<bool> EditListingEntityAsync(Guid id, EditListingInputModel listing);
        public Task<bool> DeleteListingEntityAsync(Listing listing);
        public Task<IEnumerable<ListingDto>> GetAllForSaleListingsAsync();
        public Task<IEnumerable<ListingDto>> GetAllForRentListingsAsync();
        public Task<IEnumerable<ListingDto>> GetAllNewEnquiriesAsync();
        public Task<IEnumerable<CategoriesDto>> GetAllCategoriesAsync();
        public Task<IEnumerable<ListingTypeDto>> GetAllListingTypesAsync();
        public Task<IEnumerable<AgentsDto>> GetAllAgentsAsync();
        public Task<bool> SyncImageRecordsInDb(Guid id, List<string> resolvedImagePaths);
    }
}
