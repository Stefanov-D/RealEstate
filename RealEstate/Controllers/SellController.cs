using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.GetViewModels;

namespace RealEstate.Controllers
{
    public class SellController : Controller
    {
        private readonly IListingService listingService;

        public SellController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateEnquiry()
        {
            CreateNewEnquiryInputViewModel model = await this.listingService.GetCreateNewEnquiryInputViewModelAsync();

            return View(model);
        }  

        [HttpPost]
        public async Task<IActionResult> CreateEnquiry(CreateNewEnquiryInputViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                CreateNewEnquiryInputViewModel viewModel = await listingService.GetCreateNewEnquiryInputViewModelAsync();

                // Keep the user's input values
                viewModel.Input = obj.Input;

                return View(viewModel);
            }

            bool newEnquiryIsCreated = await this.listingService.CreateNewEnquiryAsync(obj.Input);

            if (!newEnquiryIsCreated)
            {
                return View(obj);
            }

            TempData["SuccessMessage"] = "Your enquiry has been successfully submitted!";

            return RedirectToAction(nameof(CreateEnquiry));
        }

        // Controller: Add async image upload endpoint
        [HttpPost]
        public async Task<IActionResult> UploadTempImage(List<IFormFile> images)
        {
            var uploadedPaths = new List<string>();

            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(tempFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    uploadedPaths.Add("/uploads/temp/" + fileName);
                }
            }

            return Json(new { success = true, paths = uploadedPaths });
        }
    }
}
