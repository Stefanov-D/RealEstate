using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Controllers
{
    public class RentController : Controller
    {
        private readonly IListingService propertyService;
        public RentController(IListingService service)
        {
            propertyService = service;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await propertyService.GetAllForRentListingViewModelsAsync();

            return View(properties);
        }


        public async Task<IActionResult> ListingDetails(Guid id)
        {
            var property = await propertyService.GetListingViewModelByIdAsync(id);

            return View(property);
        }
    }
}
