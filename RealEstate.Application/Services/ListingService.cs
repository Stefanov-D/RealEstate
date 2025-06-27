using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.GetViewModels;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingDataAccess dataAccess;

        public ListingService(IListingDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<bool> CreateListingAsync(CreateListingInputModel model)
        {
            ListingDto listingToCreate = new ListingDto()
            {
                Title = model.Title,
                ListingTypeId = model.ListingTypeId,
                Price = model.Price,
                Street = model.Street,
                City = model.City,
                ZipCode = model.ZipCode,
                Description = model.Description,
                CategoryId = model.CategoryId,
                AgentId = model.AgentId,
                Images = model.UploadedImagePaths
            };           

            // 3. Save to database
            return await dataAccess.CreateListingEntityAsync(listingToCreate);
        }

        public async Task<bool> DeleteListingAsync(Guid id, string webRootPath)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            Listing? listingToBeDeleted = await this.dataAccess.GetListingEntityByIdAsync(id);

            if (listingToBeDeleted == null)
            {
                return false;
            }

            // Delete images
            foreach (var relativePath in listingToBeDeleted.Images)
            {
                if (string.IsNullOrWhiteSpace(relativePath.ImageUrl)) continue;

                try
                {
                    var cleanedPath = relativePath.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                    // Combine wwwroot with the stored path (e.g., "uploads/image.jpg")
                    var fullPath = Path.Combine(webRootPath, cleanedPath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    // Optional: log error or handle it
                    // _logger.LogError(ex, $"Could not delete file: {relativePath}");
                }
            }

            await this.dataAccess.DeleteListingEntityAsync(listingToBeDeleted);

            return true;
        }

        public async Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync()
        {
            IEnumerable<ListingDto> listingsForSale = await this.dataAccess.GetAllForSaleListingsAsync();

            List<ListingViewModel> listings = listingsForSale
                .Select(dto => new ListingViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Description = dto.Description,
                    Category = dto.Category,
                    Images = dto.Images
                })
            .ToList();

            return listings;
        }

        public async Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync()
        {
            IEnumerable<ListingDto> listingDto = await this.dataAccess.GetAllForRentListingsAsync();

            List<ListingViewModel> listOfProperties = listingDto
                .Select(dto => new ListingViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Description = dto.Description,
                    Category = dto.Category,
                    Images = dto.Images
                })
                .ToList();

            return listOfProperties;
        }

        public async Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id)
        {
            Listing? listing = await this.dataAccess.GetListingEntityByIdAsync(id);

            ListingDetailsViewModel listingInfo = new ListingDetailsViewModel()
            {
                Id = listing.Id,
                Title = listing.Title,
                Price = listing.Price,
                Description = listing.Description!,
                CategoryId = listing.CategoryId,
                ListingTypeId = listing.ListingTypeId,
                City = listing.Address.City,
                Street = listing.Address.Street,
                ZipCode = listing.Address.ZipCode,
                Images = listing.Images
                        .OrderByDescending(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .ToList()
            };

            return listingInfo;
        }

        public Task<List<ListingViewModel>> GetAllListingsViewModelAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> EditListingAsync(Guid id, EditListingInputModel obj)
        {
            if (obj == null)
                return false;

            // Validate model
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);
            if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
                return false;

            // Step 1: Move images first
            var resolvedImagePaths = await this.MoveUploadedImagesAsync(obj.UploadedImagePaths);
            if (resolvedImagePaths == null)
                return false;

            obj.UploadedImagePaths = resolvedImagePaths;

            try
            {
                // Step 2: Update listing fields
                bool updated = await this.dataAccess.EditListingEntityAsync(id, obj);
                if (!updated)
                    return false;

                // Step 3: Sync images using final URLs
                bool synced = await this.dataAccess.SyncImageRecordsInDb(id, resolvedImagePaths);
                return synced;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing listing: {ex.Message}");
                return false;
            }
        }

        private async Task<List<string>?> MoveUploadedImagesAsync(List<string>? uploadedImagePaths)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var tempPath = Path.Combine(wwwrootPath, "uploads", "temp");
            var permanentPath = Path.Combine(wwwrootPath, "uploads", "properties");

            if (!Directory.Exists(permanentPath))
                Directory.CreateDirectory(permanentPath);

            var resolvedImagePaths = new List<string>();

            foreach (var tempUrl in uploadedImagePaths ?? new List<string>())
            {
                try
                {
                    var fileName = Path.GetFileName(tempUrl);

                    if (tempUrl.StartsWith("/images/") || tempUrl.StartsWith("/uploads/properties/"))
                    {
                        resolvedImagePaths.Add(tempUrl);
                        continue;
                    }

                    var tempFile = Path.Combine(tempPath, fileName);
                    var newFile = Path.Combine(permanentPath, fileName);
                    var newUrl = "/uploads/properties/" + fileName;

                    if (System.IO.File.Exists(tempFile))
                    {
                        System.IO.File.Move(tempFile, newFile);
                    }

                    resolvedImagePaths.Add(newUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving image: {ex.Message}");
                    return null;
                }
            }

            return resolvedImagePaths;
        }


        public EditListingInputModel? GetUpdateListingInputModelByIdAsync(ListingDetailsViewModel model)
        {
            EditListingInputModel? inputModel = new EditListingInputModel
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                City = model.City,
                ZipCode = model.ZipCode,
                Street = model.Street,
                CategoryId = model.CategoryId,
                ListingTypeId = model.ListingTypeId,
                UploadedImagePaths = model.Images
            };

            return inputModel;
        }

        public async Task<List<ListingViewModel>> GetAllListingsViewModel()
        {
            IEnumerable<ListingDto> listings = await this.dataAccess.GetAllListingsAsync();

            List<ListingViewModel> result = listings
                .Select(dto => new ListingViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Description = dto.Description,
                    Category = dto.Category,
                    Images = dto.Images
                })
                .ToList();

            return result;
        }

        public async Task<CreateListingInputViewModel> GetCreateListingInputViewModelAsync()
        {
            IEnumerable<CategoriesDto> categories = await this.dataAccess.GetAllCategoriesAsync();
            IEnumerable<ListingTypeDto> listingTypes = await this.dataAccess.GetAllListingTypesAsync();
            IEnumerable<AgentsDto> agents = await this.dataAccess.GetAllAgentsAsync();

            var categoriesSelectList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var listingTypesSelectList = listingTypes.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var agentsSelectList = agents.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return new CreateListingInputViewModel
            {
                Categories = categoriesSelectList,
                ListingTypes = listingTypesSelectList,
                Agents = agentsSelectList,
                Input = new CreateListingInputModel()
            };
        }

        public async Task<CreateNewEnquiryInputViewModel> GetCreateNewEnquiryInputViewModelAsync()
        {
            IEnumerable<CategoriesDto> categories = await this.dataAccess.GetAllCategoriesAsync();
            IEnumerable<ListingTypeDto> listingTypes = await this.dataAccess.GetAllListingTypesAsync();
            IEnumerable<AgentsDto> agents = await this.dataAccess.GetAllAgentsAsync();

            var categoriesSelectList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var listingTypesSelectList = listingTypes.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var agentsSelectList = agents.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return new CreateNewEnquiryInputViewModel
            {
                Categories = categoriesSelectList,
                ListingTypes = listingTypesSelectList,
                Agents = agentsSelectList,
                Input = new CreateNewEnquiryInputModel()
            };
        }

        public async Task<EditListingInputViewModel> GetEditListingInputViewModelAsync(Guid id)
        {
            IEnumerable<CategoriesDto> categories = await this.dataAccess.GetAllCategoriesAsync();
            IEnumerable<ListingTypeDto> listingTypes = await this.dataAccess.GetAllListingTypesAsync();
            IEnumerable<AgentsDto> agents = await this.dataAccess.GetAllAgentsAsync();

            var categoriesSelectList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var listingTypesSelectList = listingTypes.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var agentsSelectList = agents.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            Listing? listing = await this.dataAccess.GetListingEntityByIdAsync(id);

            return new EditListingInputViewModel
            {
                Categories = categoriesSelectList,
                ListingTypes = listingTypesSelectList,
                Agents = agentsSelectList,
                Input = new EditListingInputModel
                {
                    Title = listing.Title,
                    Price = listing.Price,
                    Description = listing.Description,
                    City = listing.Address.City,
                    ZipCode = listing.Address.ZipCode,
                    Street = listing.Address.Street,
                    CategoryId = listing.CategoryId,
                    ListingTypeId = listing.ListingTypeId,
                    UploadedImagePaths = listing.Images.Select(i => i.ImageUrl).ToList()
                }
            };
        }

        public async Task<List<ListingViewModel>> GetAllNewEnquiriesViewModelAsync()
        {
            IEnumerable<ListingDto> newEnquiriesDto = await this.dataAccess
                .GetAllNewEnquiriesAsync();

            List<ListingViewModel> newEnquiriesViewModel = newEnquiriesDto
                .Select(ne => new ListingViewModel
                {
                    Id = ne.Id,
                    Title = ne.Title,
                    Price = ne.Price,
                    Description = ne.Description,
                    City = ne.City,
                    Street = ne.Street,
                    ZipCode = ne.ZipCode,
                    CategoryId = ne.CategoryId,
                    Category = ne.Category,
                    ListingTypeId = ne.ListingTypeId,
                    ListingType = ne.ListingType,
                    Images = ne.Images
                })
                .ToList();

            return newEnquiriesViewModel;
        }

        public async Task<bool> CreateNewEnquiryAsync(CreateNewEnquiryInputModel model)
        {
            ListingDto enquiry = new ListingDto()
            {
                Title = model.Title,
                ListingTypeId = model.ListingTypeId,
                Price = model.Price,
                Street = model.Street,
                City = model.City,
                ZipCode = model.ZipCode,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Images = model.UploadedImagePaths
            };

            await this.dataAccess.CreateListingEntityAsync(enquiry);

            return true;
        }
    }
}
