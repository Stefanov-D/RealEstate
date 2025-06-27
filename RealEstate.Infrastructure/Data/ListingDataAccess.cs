using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data
{
    public class ListingDataAccess : IListingDataAccess
    {
        private readonly ApplicationDbContext db;

        public ListingDataAccess(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateListingEntityAsync(ListingDto enquiry)
        {
            Listing listing = new Listing()
            {
                Title = enquiry.Title,
                Price = enquiry.Price,
                Description = enquiry.Description,
                Address = new Address()
                {
                    City = enquiry.City,
                    Street = enquiry.Street,
                    ZipCode = enquiry.ZipCode
                },
                CategoryId = enquiry.CategoryId,
                ListingTypeId = enquiry.ListingTypeId
            };

            // 2. Move temp images to permanent folder and create DB records
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var tempPath = Path.Combine(wwwrootPath, "uploads", "temp");
            var permanentPath = Path.Combine(wwwrootPath, "uploads", "properties");

            if (!Directory.Exists(permanentPath))
                Directory.CreateDirectory(permanentPath);

            bool isFirstImage = true;

            foreach (var tempUrl in enquiry.Images)
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

                    listing.Images.Add(new Image()
                    {
                        ImageUrl = newUrl,
                        IsPrimary = isFirstImage
                    });

                    isFirstImage = false; // only the first image gets true
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            await this.db.Listings.AddAsync(listing);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteListingEntityAsync(Listing listing)
        {
            db.Listings.Remove(listing);

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ListingDto>> GetAllForSaleListingsAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Agent)
                .Where(p => p.Category.Name == "For Sale" && p.IsNewEnquiry == false)
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingDto
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
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ListingDto>> GetAllForRentListingsAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Agent)
                .Where(p => p.Category.Name == "For Rent" && p.IsNewEnquiry == false)
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingDto
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
                .ToArrayAsync();
        }

        public async Task<Listing?> GetListingEntityByIdAsync(Guid id)
        {
            Listing? listing = await db.Listings
                .Include(l => l.Address)
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            return listing;
        }

        public async Task<bool> EditListingEntityAsync(Guid id, EditListingInputModel model)
        {
            var listingToUpdate = await db.Listings
                .Include(l => l.Address)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (model == null || listingToUpdate == null)
                return false;

            listingToUpdate.Title = model.Title;
            listingToUpdate.Price = model.Price;
            listingToUpdate.Address.City = model.City;
            listingToUpdate.Address.Street = model.Street;
            listingToUpdate.Address.ZipCode = model.ZipCode;
            listingToUpdate.CategoryId = model.CategoryId;
            listingToUpdate.ListingTypeId = model.ListingTypeId;

            return true; // Don't call SaveChanges here; defer to final step.
        }


        public async Task<IEnumerable<ListingDto>> GetAllListingsAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Agent)
                .Where(p => p.IsNewEnquiry == false)
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingDto
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
                .ToArrayAsync();
        }

        public async Task<IEnumerable<CategoriesDto>> GetAllCategoriesAsync()
        {
            return await db.Categories
                .AsNoTracking()
                .Select(c => new CategoriesDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ListingTypeDto>> GetAllListingTypesAsync()
        {
            return await db.ListingTypes
               .AsNoTracking()
               .Select(c => new ListingTypeDto
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToListAsync();
        }

        public async Task<IEnumerable<AgentsDto>> GetAllAgentsAsync()
        {
            return await db.Agents
               .AsNoTracking()
               .Select(c => new AgentsDto
               {
                   Id = c.Id,
                   Name = $"{c.FirstName} {c.LastName}"
               })
               .ToListAsync();
        }

        public async Task<bool> SyncImageRecordsInDb(Guid id, List<string> resolvedImagePaths)
        {
            // Ensure eager loading of Images
            var listing = await db.Listings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null)
                return false;

            var normalizedPaths = resolvedImagePaths.Select(p => p.Trim()).ToList();

            // 1. REMOVE old images
            var existingImages = listing.Images.ToList(); // ensure materialization

            foreach (var image in existingImages)
            {
                if (!normalizedPaths.Contains(image.ImageUrl))
                {
                    db.Images.Remove(image); // ← FIRST remove from DbContext
                    listing.Images.Remove(image); // optional, to keep collection clean
                }
            }

            // 2. ADD new images
            var existingUrls = existingImages.Select(i => i.ImageUrl).ToHashSet();

            var imagesToAdd = normalizedPaths
                .Where(url => !existingUrls.Contains(url))
                .ToList();

            foreach (var path in imagesToAdd)
            {
                db.Images.Add(new Image
                {
                    ListingId = listing.Id,
                    ImageUrl = path,
                    IsPrimary = false
                });
            }

            // 3. UPDATE IsPrimary: make first matching one primary
            foreach (var image in listing.Images)
                image.IsPrimary = false;

            var firstPrimary = listing.Images
                .FirstOrDefault(img => normalizedPaths.Contains(img.ImageUrl));

            if (firstPrimary != null)
                firstPrimary.IsPrimary = true;

            // 4. Save
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing images: {ex.Message}");
                return false;
            }
        }


        public async Task<IEnumerable<ListingDto>> GetAllNewEnquiriesAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .Include(l => l.Agent)
                .Where(p => p.IsNewEnquiry == true)
                .OrderByDescending(p => p.Price)
                .Select(p => new ListingDto
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
        }
    }
}