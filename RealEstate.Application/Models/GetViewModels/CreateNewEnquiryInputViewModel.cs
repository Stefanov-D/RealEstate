using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Application.Models.PostInputModels;

namespace RealEstate.Application.Models.GetViewModels
{
    public class CreateNewEnquiryInputViewModel
    {
        public CreateNewEnquiryInputModel Input { get; set; } = new CreateNewEnquiryInputModel();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ListingTypes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Agents { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
