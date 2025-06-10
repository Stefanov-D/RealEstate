using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Controllers
{
    public class AddListingController : Controller
    {
        private readonly ApplicationDbContext db;
        readonly IListingService propertyService;
        public AddListingController(IListingService service, ApplicationDbContext dbContext)
        {
            propertyService = service;
            db = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> AddListing()
        {
            ViewBag.Categories = new SelectList(await this.db.Categories.Select(a => new { a.Id, a.Name }).ToListAsync(), "Id", "Name");
            ViewBag.ListingTypes = new SelectList(await this.db.ListingTypes.Where(t => t.Name != "Buy").OrderBy(t => t.Id).Select(a => new {a.Id, a.Name}).ToListAsync(), "Id", "Name");
            ViewBag.Agents = new SelectList(
                    await db.Agents
                        .Select(a => new { a.Id, FullName = a.FirstName + " " + a.LastName })
                        .ToListAsync(), "Id", "FullName");

            var model = new CreateListingInputModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddListing(CreateListingInputModel obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await db.Categories.ToListAsync(), "Id", "Name");
                ViewBag.ListingTypes = new SelectList(await db.ListingTypes.Where(t => t.Name != "Buy").OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

                // Make sure this is not null to avoid runtime errors in the view
                if (obj.UploadedImagePaths == null)
                    obj.UploadedImagePaths = new List<string>();

                return View(obj);
            }

            Listing property = new Listing()
            {
                Title = obj.Title,
                ListingTypeId = obj.ListingTypeId,
                Price = obj.Price,
                Address = new Address
                {
                    Street = obj.Street,
                    City = obj.City,
                    ZipCode = obj.ZipCode,
                },
                Description = obj.Description,
                CategoryId = obj.CategoryId,
                AgentId = obj.AgentId,
                Images = new List<Image>()
            };
            // 2. Move temp images to permanent folder and create DB records
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var tempPath = Path.Combine(wwwrootPath, "uploads", "temp");
            var permanentPath = Path.Combine(wwwrootPath, "uploads", "properties");

            if (!Directory.Exists(permanentPath))
                Directory.CreateDirectory(permanentPath);

            foreach (var tempUrl in obj.UploadedImagePaths)
            {
                var fileName = Path.GetFileName(tempUrl); // only filename, e.g., abc123.jpg
                var tempFile = Path.Combine(tempPath, fileName);
                var newFile = Path.Combine(permanentPath, fileName);
                var newUrl = "/uploads/properties/" + fileName;

                try
                {
                    if (System.IO.File.Exists(tempFile))
                    {
                        System.IO.File.Move(tempFile, newFile);
                    }

                    property.Images.Add(new Image()
                    {
                        ImageUrl = newUrl,
                        IsPrimary = false ? true : false
                    });
                }
                catch (Exception ex)
                {
                    // Optionally: log and skip or rethrow
                    ModelState.AddModelError("", "Error processing images. Please try again.");
                    return View(nameof(AddListing), obj);
                }
            }

            // 3. Save to database
            db.Listings.Add(property);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your enquiry has been successfully submitted!";

            return RedirectToAction(nameof(AddListing));
        }
    }
}
