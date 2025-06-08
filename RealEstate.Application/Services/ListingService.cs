using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IApplicationDbContext db;

        public ListingService(IApplicationDbContext context)
        {
            db = context;
        }
        public Task<int> CreateListingAsync(CreateListingInputModel dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            Listing? listingToBeDeleted = await GetListingByIdAsync(id);

            if (listingToBeDeleted == null)
            {
                return false;
            }

            db.Listings.Remove(listingToBeDeleted);

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync()
        {
            var listOfProperties = await db.Listings
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Category.Name == "For Sale")
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

            return listOfProperties;
        }

        public async Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync()
        {
            var listOfProperties = await db.Listings
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Category.Name == "For Rent")
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

            return listOfProperties;
        }

        public async Task<Listing?> GetListingByIdAsync(Guid id)
        {
            Listing? listing = await db.Listings.Include(l => l.Address)
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            return listing;
        }

        public async Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id)
        {
            var propertyInfo = await db.Listings
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.ListingType)
                .Include(p => p.Images)
                .Include(p => p.Address)
                .Select(p => new ListingDetailsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description!,
                    CategoryId = p.CategoryId,
                    ListingTypeId = p.ListingTypeId,
                    City = p.Address.City,
                    Street = p.Address.Street,
                    ZipCode = p.Address.ZipCode,
                    Images = p.Images
                        .OrderByDescending(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return propertyInfo;
        }

        public Task<List<ListingViewModel>> GetAllListingsViewModelAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateListingAsync(UpdateListingInputModel obj, Listing listingToUpdate)
        {
            if (obj == null || listingToUpdate == null)
                return false;

            // Validate model
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
                return false;

            // Step 1: Update listing fields
            listingToUpdate.Title = obj.Title;
            listingToUpdate.Price = obj.Price;
            listingToUpdate.Description = obj.Description;
            listingToUpdate.Address.City = obj.City;
            listingToUpdate.Address.Street = obj.Street;
            listingToUpdate.Address.ZipCode = obj.ZipCode;
            listingToUpdate.CategoryId = obj.CategoryId;
            listingToUpdate.ListingTypeId = obj.ListingTypeId;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving listing info: {ex.Message}");
                return false;
            }

            // Step 2: Move image files from temp to permanent folder and build new URLs
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var tempPath = Path.Combine(wwwrootPath, "uploads", "temp");
            var permanentPath = Path.Combine(wwwrootPath, "uploads", "properties");

            if (!Directory.Exists(permanentPath))
                Directory.CreateDirectory(permanentPath);

            var resolvedImagePaths = new List<string>();

            foreach (var tempUrl in obj.UploadedImagePaths ?? new List<string>())
            {
                var fileName = Path.GetFileName(tempUrl);
                var tempFile = Path.Combine(tempPath, fileName);
                var newFile = Path.Combine(permanentPath, fileName);
                var newUrl = "/uploads/properties/" + fileName;

                try
                {
                    if (System.IO.File.Exists(tempFile))
                    {
                        System.IO.File.Move(tempFile, newFile);
                    }
                    resolvedImagePaths.Add(newUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving image: {ex.Message}");
                    return false;
                }
            }

            // Step 3: Sync image records in DB

            // Remove images no longer in the new list
            var imagesToRemove = listingToUpdate.Images.Where(i => !resolvedImagePaths.Contains(i.ImageUrl)).ToList();
            foreach (var image in imagesToRemove)
            {
                listingToUpdate.Images.Remove(image);
                db.Images.Remove(image);
            }

            // Add new images not already in DB
            var existingPaths = listingToUpdate.Images.Select(i => i.ImageUrl).ToHashSet();
            var imagesToAdd = resolvedImagePaths.Where(p => !existingPaths.Contains(p));

            foreach (var path in imagesToAdd)
            {
                var newImage = new Image()
                {
                    ListingId = listingToUpdate.Id,
                    ImageUrl = path,
                    IsPrimary = false
                };
                listingToUpdate.Images.Add(newImage);
                db.Images.Add(newImage);
            }

            // Mark only the first image as primary
            bool primarySet = false;
            foreach (var img in listingToUpdate.Images)
            {
                if (!primarySet)
                {
                    img.IsPrimary = true;
                    primarySet = true;
                }
                else
                {
                    img.IsPrimary = false;
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving images: {ex.Message}");
                return false;
            }

            return true;
        }

    }
}
