using RealEstate.Application.Models.GetViewModels;
using RealEstate.Application.Models.PostInputModels;

namespace RealEstate.Application.Interfaces
{
    public interface IListingService
    {        
        Task<bool> CreateListingAsync(CreateListingInputModel dto);

        public Task<CreateListingInputViewModel> GetCreateListingInputViewModelAsync();

        public Task<EditListingInputViewModel> GetEditListingInputViewModelAsync(Guid id);

        Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id);

        EditListingInputModel? GetUpdateListingInputModelByIdAsync(ListingDetailsViewModel model);

        Task<List<ListingViewModel>> GetAllListingsViewModelAsync();

        Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync();

        Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync();

        Task<bool> EditListingAsync(Guid id, EditListingInputModel inputModel);

        Task<bool> DeleteListingAsync(Guid id, string WebRootPath);

        public Task<List<ListingViewModel>> GetAllListingsViewModel();

        public Task<List<ListingViewModel>> GetAllNewEnquiriesViewModelAsync();

        public Task<bool> CreateNewEnquiryAsync(CreateNewEnquiryInputModel model);

        public Task<CreateNewEnquiryInputViewModel> GetCreateNewEnquiryInputViewModelAsync();
    }
}
