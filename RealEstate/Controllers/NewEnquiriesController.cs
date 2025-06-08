using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewEnquiriesController : Controller
    {
        private readonly ApplicationDbContext db;
        public NewEnquiriesController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            var listOfProperties = await db.Listings
                .Where(p => p.Category.Name == "To Sell")
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingViewModel
                {
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description!,
                    Category = p.Category.Name,
                    Images = p.Images
                        .OrderByDescending(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .ToList()
                })
                .ToListAsync();


            return View(listOfProperties);
        }

        public async Task<IActionResult> PostDetails(Guid id)
        {
            var postInfo = await db.Listings
                .Where(p => p.Id == id)
                .Select(p => new ListingViewModel
                {
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description!,
                    Category = p.Category.Name,
                    Images = p.Images
                        .OrderByDescending(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return View(postInfo);
        }
    }
}
