using RealEstate.Application.Models.PostInputModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstate.Application.Models.GetViewModels
{
    public class CreateListingInputViewModel
    {
        public CreateListingInputModel Input { get; set; } = new CreateListingInputModel();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ListingTypes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Agents { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
