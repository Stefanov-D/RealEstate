using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Controllers
{
    public class ListingController : Controller
    {
        readonly IListingService propertyService;
        private readonly ApplicationDbContext db;
        public ListingController(IListingService service,ApplicationDbContext context)
        {
            propertyService = service;
            db = context;
        }

        public IListingService PropertyService => propertyService;
        public async Task<IActionResult> Index(Guid id)
        {
            var listing = await propertyService.GetListingViewModelByIdAsync(id);

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Categories = new SelectList(await this.db.Categories.ToListAsync(), "Id", "Name");
            ViewBag.ListingTypes = new SelectList(await this.db.ListingTypes.OrderBy(t => t.Id).ToListAsync(), "Id", "Name");
            var listing = await propertyService.GetListingViewModelByIdAsync(id);

            if (listing == null)
            {
                return NotFound();
            }

            var inputModel = new UpdateListingInputModel
            {    
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                City = listing.City,
                ZipCode = listing.ZipCode,
                Street = listing.Street,
                CategoryId = listing.CategoryId,
                ListingTypeId = listing.ListingTypeId,
                UploadedImagePaths = listing.Images
            };

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateListingInputModel obj)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, make sure to repopulate dropdowns
                ViewBag.Categories = new SelectList(await db.Categories.ToListAsync(), "Id", "Name");
                ViewBag.ListingTypes = new SelectList(await db.ListingTypes.OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

                // Return view with current model (including uploaded images)
                return View(obj);
            }

            ViewBag.Categories = new SelectList(await db.Categories.ToListAsync(), "Id", "Name");
            ViewBag.ListingTypes = new SelectList(await db.ListingTypes.OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

            // Fetch the listing entity to update
            var listingToUpdate = await propertyService.GetListingByIdAsync(id);
            if (listingToUpdate == null)
            {
                return NotFound();
            }

            var success = await propertyService.UpdateListingAsync(obj, listingToUpdate);
            if (!success)
            {
                ModelState.AddModelError("", "Update failed. Try again.");
                return View(obj);
            }

            // After successful update, reload the model from database including images
            var updatedListing = await propertyService.GetListingViewModelByIdAsync(id);

            if (updatedListing == null)
            {
                return NotFound();
            }

            var inputModel = new UpdateListingInputModel
            {
                Title = updatedListing.Title,
                Description = updatedListing.Description,
                Price = updatedListing.Price,
                City = updatedListing.City,
                ZipCode = updatedListing.ZipCode,
                Street = updatedListing.Street,
                CategoryId = updatedListing.CategoryId,
                ListingTypeId = updatedListing.ListingTypeId,
                UploadedImagePaths = updatedListing.Images // <-- Make sure this is populated with the image URLs!
            };

            return View(inputModel);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted = await propertyService.DeleteListingAsync(id);

            if (!isDeleted)
            {
                RedirectToAction("ListingDetails");
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
