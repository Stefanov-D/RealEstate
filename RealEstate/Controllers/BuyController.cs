using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;

namespace RealEstate.Controllers
{
    public class BuyController : Controller
    {
        private readonly IListingService listingService;
        public BuyController(IListingService service)
        {
            listingService = service;
        }
        
        public async Task<IActionResult> Index()
        {
            var listings = await listingService.GetAllForSaleListingViewModelsAsync();
            return View(listings);
        }


        public async Task<IActionResult> ListingDetails(Guid id)
        {
            var listingInfo = await listingService.GetListingViewModelByIdAsync(id);

            return View(listingInfo);
        }
    }
}
