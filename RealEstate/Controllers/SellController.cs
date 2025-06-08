using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Controllers
{
    public class SellController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SellController(ApplicationDbContext db/*, ILogger logger*/)
        {
            this._db = db;
            /*this._logger = logger;*/
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.PropertyTypes = new SelectList(await this._db.Categories.ToListAsync(), "Id", "Name");
            ViewBag.ListingTypes = new SelectList(await this._db.ListingTypes.Where(t => t.Name != "Buy").OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

            var model = new CreateListingInputModel
            {
                UploadedImagePaths = new List<string>() // initialize to avoid null in the view
            };

            return View(model);
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


        [HttpPost]
        public async Task<IActionResult> Create(CreateListingInputModel obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PropertyTypes = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
                ViewBag.ListingTypes = new SelectList(await _db.ListingTypes.Where(t => t.Name != "Buy").OrderBy(t => t.Id).ToListAsync(), "Id", "Name");

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
                Address = new Address {
                    Street = obj.Street,
                    City = obj.City,
                    ZipCode = obj.ZipCode,
                },
                Description = obj.Description,
                CategoryId = obj.CategoryId,
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
                        IsPrimary = false? true : false
                    });
                }
                catch (Exception ex)
                {
                    // Optionally: log and skip or rethrow
                    ModelState.AddModelError("", "Error processing images. Please try again.");
                    return View(nameof(Create), obj);
                }
            }

            // 3. Save to database
            _db.Listings.Add(property);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Create));
        }
    }
}
