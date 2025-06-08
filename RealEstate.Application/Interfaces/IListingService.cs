using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IListingService
    {
        Task<Listing> GetListingByIdAsync(Guid id);
        Task<int> CreateListingAsync(CreateListingInputModel dto);

        Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id);

        Task<List<ListingViewModel>> GetAllListingsViewModelAsync();

        Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync();

        Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync();

        Task<bool> UpdateListingAsync(UpdateListingInputModel inputModel, Listing listingToUpdate);

        Task<bool> DeleteListingAsync(Guid id);
    }
}
