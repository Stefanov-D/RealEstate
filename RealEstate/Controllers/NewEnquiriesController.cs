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
                .AsNoTracking()
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .Include(l => l.Agent)
                .Where(p => p.IsNewEnquiry == true)
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingViewModel
                {
                    Id = p.Id,
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

        public async Task<IActionResult> ListingDetails(Guid id)
        {
            var postInfo = await db.Listings
                .AsNoTracking()
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .Include(l => l.Agent)
                .Where(p => p.Id == id)
                .Select(p => new ListingDetailsViewModel
                {
                    Id = p.Id,
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

            if (postInfo == null)
            {
                return NotFound();
            }

            return View(postInfo);
        }
    }
}
