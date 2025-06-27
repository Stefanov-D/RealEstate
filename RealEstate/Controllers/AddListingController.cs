using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.GetViewModels;

namespace RealEstate.Controllers
{
    public class AddListingController : Controller
    {
        private readonly IListingService listingService;
        public AddListingController(IListingService service)
        {
            this.listingService = service;
        }

        [HttpGet]
        public async Task<IActionResult> AddListing()
        {            
            CreateListingInputViewModel model = await this.listingService.GetCreateListingInputViewModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddListing(CreateListingInputViewModel obj) 
        {
            if (!ModelState.IsValid)
            {
                CreateListingInputViewModel viewModel = await listingService.GetCreateListingInputViewModelAsync();

                // Keep the user's input values
                viewModel.Input = obj.Input;

                return View(viewModel);
            }

            bool success = await this.listingService.CreateListingAsync(obj.Input);  
            
            if (!success)
            {
                return View(nameof(AddListing), obj);
            }

            TempData["SuccessMessage"] = "Your enquiry has been successfully submitted!";
            return RedirectToAction(nameof(AddListing));
        }
    }
}
