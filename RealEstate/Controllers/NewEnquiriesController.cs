using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;

namespace RealEstate.Controllers
{
    [Authorize(Roles = "Admin,Agent")]
    public class NewEnquiriesController : Controller
    {
        private readonly IListingService listingService;

        public NewEnquiriesController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listOfProperties = await this.listingService.GetAllNewEnquiriesViewModelAsync();   

            return View(listOfProperties);
        }

        [HttpGet]
        public async Task<IActionResult> ListingDetails(Guid id)
        {
            var listingInfo = await listingService.GetListingViewModelByIdAsync(id);

            if (listingInfo == null)
            {
                return NotFound();
            }

            return View(listingInfo);
        }
    }
}
