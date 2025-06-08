using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Controllers
{
    public class BuyController : Controller
    {
        private readonly IListingService propertyService;
        public BuyController(IListingService service)
        {
            propertyService = service;
        }
        
        public async Task<IActionResult> Index()
        {
            var listings = await propertyService.GetAllForSaleListingViewModelsAsync();
            return View(listings);
        }


        public async Task<IActionResult> ListingDetails(Guid id)
        {
            var listingInfo = await propertyService.GetListingViewModelByIdAsync(id);

            return View(listingInfo);
        }
    }
}
