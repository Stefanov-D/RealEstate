using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.GetViewModels;

namespace RealEstate.Controllers
{
    public class ListingController : Controller
    {
        private readonly IListingService listingService;
        public ListingController(IListingService service, IWebHostEnvironment webHostEnvironment)
        {
            listingService = service;
        }
        public async Task<IActionResult> Index(Guid id)
        {
            var listing = await listingService.GetListingViewModelByIdAsync(id);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            EditListingInputViewModel? model = await listingService.GetEditListingInputViewModelAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditListingInputViewModel obj)
        {
            if (!ModelState.IsValid || !await listingService.EditListingAsync(id, obj.Input))
            {
                EditListingInputViewModel viewModel = await listingService.GetEditListingInputViewModelAsync(id);

                viewModel.Input = obj.Input;

                return View(viewModel);
            }            

            // After successful update, reload the model from database including images
            var updatedListing = await listingService.GetEditListingInputViewModelAsync(id);

            if (updatedListing == null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Listing successfully updated!";

            return View(updatedListing);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted = await listingService.DeleteListingAsync(id);

            if (!isDeleted)
            {
                return View("ListingDetails", id);
            }

            return View("DeletedSuccess");
        }

        [HttpPost]
        public async Task<IActionResult> UploadTempImage(List<IFormFile> images)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var tempPath = Path.Combine(wwwrootPath, "uploads", "temp");

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            var savedPaths = new List<string>();

            foreach (var file in images)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(tempPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    savedPaths.Add("/uploads/temp/" + fileName);
                }
            }

            return Json(new { success = true, paths = savedPaths });
        }
    }
}
