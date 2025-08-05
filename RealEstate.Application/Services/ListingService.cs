using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.GetViewModels;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IFileStorageService fileStorageService;
        private readonly IListingTypeRepository listingTypeRepository;
        private readonly IAgentRepository agentRepository;
        private readonly IImageRepository imageRepository;

        public ListingService(IListingRepository repository,
            IFileStorageService fileStorageService,
            ICategoryRepository categoryRepository,
            IListingTypeRepository listingTypeRepository,
            IAgentRepository agentRepository,
            IImageRepository imageRepository)
        {
            this.repository = repository;
            this.fileStorageService = fileStorageService;
            this.categoryRepository = categoryRepository;
            this.listingTypeRepository = listingTypeRepository;
            this.agentRepository = agentRepository;
            this.imageRepository = imageRepository;
        }

        public async Task<bool> CreateListingAsync(CreateListingInputModel model)
        {
            var resolvedImages = await fileStorageService.MoveImagesFromTempAsync(model.UploadedImagePaths);

            Listing listingToCreate = new Listing()
            {
                Title = model.Title,
                ListingTypeId = model.ListingTypeId,
                Price = model.Price,
                Address = new Address()
                    {   
                        Street = model.Street,
                        City = model.City,
                        ZipCode = model.ZipCode,
                    },           
                Description = model.Description,
                CategoryId = model.CategoryId,
                AgentId = model.AgentId,
                Images = resolvedImages.Select((url, index) => new Image
                {
                    ImageUrl = url,
                    IsPrimary = index == 0
                }).ToList()
            };           

            await repository.AddAsync(listingToCreate);

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            Listing? listingToBeDeleted = await repository.GetListingWithDetailsAsync(id);

            if (listingToBeDeleted == null)
            {
                return false;
            }

            foreach (var image in listingToBeDeleted.Images)
            {
                await fileStorageService.DeleteImageAsync(image.ImageUrl);
            }

            await this.repository.DeleteAsync(listingToBeDeleted);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<ListingViewModel>> GetAllForSaleListingViewModelsAsync()
        {
            var listingsForSale = await repository.GetAllForSaleAsync();

            var viewModels = listingsForSale
                .Select(dto => new ListingViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Description = dto.Description,
                    City = dto.Address?.City ?? string.Empty,
                    Street = dto.Address?.Street ?? string.Empty,
                    ZipCode = dto.Address?.ZipCode,
                    CategoryId = dto.CategoryId,
                    Category = dto.Category?.Name,
                    ListingTypeId = dto.ListingTypeId,
                    ListingType = dto.ListingType?.Name,
                    Images = dto.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
                })
                .ToList();

            return viewModels;
        }

        public async Task<List<ListingViewModel>> GetAllForRentListingViewModelsAsync()
        {
            var listingsForRent = await this.repository.GetAllForRentAsync();

            var viewModels = listingsForRent
                .Select(dto => new ListingViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Description = dto.Description,
                    City = dto.Address?.City ?? string.Empty,
                    Street = dto.Address?.Street ?? string.Empty,
                    ZipCode = dto.Address?.ZipCode,
                    CategoryId = dto.CategoryId,
                    Category = dto.Category?.Name,
                    ListingTypeId = dto.ListingTypeId,
                    ListingType = dto.ListingType?.Name,
                    Images = dto.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
                })
                .ToList();

            return viewModels;
        }

        public async Task<ListingDetailsViewModel?> GetListingViewModelByIdAsync(Guid id)
        {
            Listing? listing = await this.repository.GetListingWithDetailsAsync(id);

            ListingDetailsViewModel listingInfo = new ListingDetailsViewModel()
            {
                Id = listing.Id,
                Title = listing.Title,
                Price = listing.Price,
                Description = listing.Description!,
                CategoryId = listing.CategoryId,
                Category = listing.Category?.Name,
                ListingTypeId = listing.ListingTypeId,
                ListingType = listing.ListingType?.Name,
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

        public async Task<bool> EditListingAsync(Guid id, EditListingInputModel inputModel)
        {
            if (inputModel == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(inputModel);
            if (!Validator.TryValidateObject(inputModel, validationContext, validationResults, true))
                return false;

            try
            {
                var resolvedImagePaths = await fileStorageService.MoveImagesFromTempAsync(inputModel.UploadedImagePaths);
                var listingToUpdate = await repository.GetListingWithDetailsAsync(id);

                if (listingToUpdate == null)
                    return false;

                // Update scalar properties
                listingToUpdate.Title = inputModel.Title;
                listingToUpdate.Price = inputModel.Price;
                listingToUpdate.Description = inputModel.Description;
                listingToUpdate.CategoryId = inputModel.CategoryId;
                listingToUpdate.ListingTypeId = inputModel.ListingTypeId;

                if (listingToUpdate.Address == null)
                    listingToUpdate.Address = new Address();

                listingToUpdate.Address.City = inputModel.City;
                listingToUpdate.Address.Street = inputModel.Street;
                listingToUpdate.Address.ZipCode = inputModel.ZipCode;

                // 🔄 Remove images not in resolved list
                var imagesToRemove = listingToUpdate.Images
                    .Where(img => !resolvedImagePaths.Contains(img.ImageUrl))
                    .ToList();

                foreach (var img in imagesToRemove)
                {
                    await imageRepository.DeleteAsync(img);
                }

                // ➕ Add new images
                foreach (var path in resolvedImagePaths)
                {
                    if (!listingToUpdate.Images.Any(img => img.ImageUrl == path))
                    {
                        await imageRepository.AddAsync(new Image
                        {
                            ListingId = id,
                            ImageUrl = path,
                            IsPrimary = false
                        });
                    }
                }

                // ✅ Set IsPrimary
                foreach (var img in listingToUpdate.Images)
                    img.IsPrimary = false;

                var primaryImage = listingToUpdate.Images.FirstOrDefault(img => resolvedImagePaths.Contains(img.ImageUrl));
                if (primaryImage != null)
                    primaryImage.IsPrimary = true;

                repository.Update(listingToUpdate);
                await repository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing listing: {ex.Message}");
                return false;
            }
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

        public async Task<CreateListingInputViewModel> GetCreateListingInputViewModelAsync()
        {
            // Fetch data from repositories
            IEnumerable<Category> categories = await categoryRepository.GetAllAsync();
            IEnumerable<ListingType> listingTypes = await listingTypeRepository.GetAllAsync();
            IEnumerable<Agent> agents = await agentRepository.GetAllAsync();

            var categoriesSelectList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var listingTypesSelectList = listingTypes.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            }).ToList();

            var agentsSelectList = agents.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.FirstName} {a.LastName}"
            }).ToList();

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
            IEnumerable<Category> categories = await categoryRepository.GetAllAsync();
            IEnumerable<ListingType> listingTypes = await listingTypeRepository.GetAllAsync();
            IEnumerable<Agent> agents = await agentRepository.GetAllAsync();

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
                Text = $"{c.FirstName} {c.LastName}"
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
            IEnumerable<Category> categories = await categoryRepository.GetAllAsync();
            IEnumerable<ListingType> listingTypes = await listingTypeRepository.GetAllAsync();
            IEnumerable<Agent> agents = await agentRepository.GetAllAsync();

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
                Text = $"{c.FirstName} {c.LastName}"
            });

            Listing? listing = await this.repository.GetListingWithDetailsAsync(id);

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
            IEnumerable<Listing> newEnquiriesDto = await this.repository.GetAllNewEnquiriesAsync();

            List<ListingViewModel> newEnquiriesViewModel = newEnquiriesDto
                .Select(ne => new ListingViewModel
                {
                    Id = ne.Id,
                    Title = ne.Title,
                    Price = ne.Price,
                    Description = ne.Description,
                    City = ne.Address?.City ?? string.Empty,
                    Street = ne.Address?.Street ?? string.Empty,
                    ZipCode = ne.Address?.ZipCode,
                    CategoryId = ne.CategoryId,
                    Category = ne.Category?.Name,
                    ListingTypeId = ne.ListingTypeId,
                    ListingType = ne.ListingType?.Name,
                    Images = ne.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
                })
                .ToList();

            return newEnquiriesViewModel;
        }


        public async Task<bool> CreateNewEnquiryAsync(CreateNewEnquiryInputModel model)
        {
            var enquiry = new Listing()
            {
                Title = model.Title,
                ListingTypeId = model.ListingTypeId,
                Price = model.Price,
                Description = model.Description,
                CategoryId = model.CategoryId,
                IsNewEnquiry = true,
                Address = new Address()
                {
                    Street = model.Street,
                    City = model.City,
                    ZipCode = model.ZipCode
                },
                Images = model.UploadedImagePaths?
                            .Select(path => new Image { ImageUrl = path, IsPrimary = false })
                            .ToList() ?? new List<Image>()
            };

            await this.repository.AddAsync(enquiry);
            await this.repository.SaveChangesAsync();

            return true;
        }
    }
}
