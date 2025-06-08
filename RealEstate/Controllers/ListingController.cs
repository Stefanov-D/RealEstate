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
                return View(obj);
            }
            ViewBag.Categories = new SelectList(await db.Categories.ToListAsync(), "Id", "Name");
            ViewBag.ListingTypes = new SelectList(await db.ListingTypes.OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

            Listing? listingToUpdate = await propertyService.GetListingByIdAsync(id);

            if (listingToUpdate == null)
            {
                return View(obj);
            }

            var success = await propertyService.UpdateListingAsync(obj, listingToUpdate);
            if (!success)
            {
                ModelState.AddModelError("", "Update failed. Try again.");
                return View(obj);
            }

            return View(obj);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var listing = await propertyService.GetListingViewModelByIdAsync(id);

            return View();
        }
    }
}
