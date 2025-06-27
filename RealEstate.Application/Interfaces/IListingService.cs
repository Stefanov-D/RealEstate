using RealEstate.Application.Models.GetViewModels;
using RealEstate.Application.Models.PostInputModels;

namespace RealEstate.Application.Interfaces
{
    public interface IListingService
    {        
        public Task<bool> CreateListingAsync(CreateListingInputModel dto);
        public Task<bool> CreateNewEnquiryAsync(CreateNewEnquiryInputModel model);
        public Task<bool> EditListingAsync(Guid id, EditListingInputModel inputModel);
        public Task<bool> DeleteListingAsync(Guid id, string WebRootPath);
        public Task<CreateListingInputViewModel> GetCreateListingInputViewModelAsync();
        public Task<EditListingInputViewModel> GetEditListingInputViewModelAsync(Guid id);
        public Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id);
        public EditListingInputModel? GetUpdateListingInputModelByIdAsync(ListingDetailsViewModel model);
        public Task<List<ListingViewModel>> GetAllListingsViewModelAsync();
        public Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync();
        public Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync();        
        public Task<List<ListingViewModel>> GetAllListingsViewModel();
        public Task<List<ListingViewModel>> GetAllNewEnquiriesViewModelAsync();        
        public Task<CreateNewEnquiryInputViewModel> GetCreateNewEnquiryInputViewModelAsync();
    }
}
