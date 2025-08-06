using NUnit.Framework;
using Moq;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models.PostInputModels;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using NUnit.Framework.Legacy;

namespace RealEstate.Application.Tests.Services
{
    [TestFixture]
    public class ListingServiceTests
    {
        private Mock<IListingRepository> listingRepoMock = null!;
        private Mock<IFileStorageService> fileStorageMock = null!;
        private Mock<ICategoryRepository> categoryRepoMock = null!;
        private Mock<IListingTypeRepository> listingTypeRepoMock = null!;
        private Mock<IAgentRepository> agentRepoMock = null!;
        private Mock<IImageRepository> imageRepoMock = null!;
        private ListingService service = null!;

        [SetUp]
        public void Setup()
        {
            listingRepoMock = new Mock<IListingRepository>();
            fileStorageMock = new Mock<IFileStorageService>();
            categoryRepoMock = new Mock<ICategoryRepository>();
            listingTypeRepoMock = new Mock<IListingTypeRepository>();
            agentRepoMock = new Mock<IAgentRepository>();
            imageRepoMock = new Mock<IImageRepository>();

            service = new ListingService(
                listingRepoMock.Object,
                fileStorageMock.Object,
                categoryRepoMock.Object,
                listingTypeRepoMock.Object,
                agentRepoMock.Object,
                imageRepoMock.Object);
        }
                

        [Test]
        public async Task DeleteListingAsync_ShouldReturnFalse_WhenIdEmpty()
        {
            var result = await service.DeleteListingAsync(Guid.Empty);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteListingAsync_ShouldReturnFalse_WhenListingNull()
        {
            listingRepoMock.Setup(r => r.GetListingWithDetailsAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Listing?)null);

            var result = await service.DeleteListingAsync(Guid.NewGuid());

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteListingAsync_ShouldDeleteImagesAndListing_WhenListingExists()
        {
            var listing = new Listing
            {
                Id = Guid.NewGuid(),
                Images = new List<Image> { new Image { ImageUrl = "url1" }, new Image { ImageUrl = "url2" } }
            };

            listingRepoMock.Setup(r => r.GetListingWithDetailsAsync(listing.Id))
                .ReturnsAsync(listing);

            var result = await service.DeleteListingAsync(listing.Id);

            ClassicAssert.IsTrue(result);
            foreach (var img in listing.Images)
            {
                fileStorageMock.Verify(f => f.DeleteImageAsync(img.ImageUrl), Times.Once);
            }
            listingRepoMock.Verify(r => r.DeleteAsync(listing), Times.Once);
            listingRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllForSaleListingViewModelsAsync_ShouldReturnMappedListings()
        {
            var listings = new List<Listing>
            {
                new Listing
                {
                    Id = Guid.NewGuid(),
                    Title = "Sale 1",
                    Price = 200000,
                    Description = "Desc",
                    Address = new Address { City = "CityA", Street = "StreetA", ZipCode = 1111 },
                    Category = new Category { Name = "Cat1" },
                    CategoryId = Guid.NewGuid(),
                    ListingType = new ListingType { Name = "Sale" },
                    ListingTypeId = Guid.NewGuid(),
                    Images = new List<Image> { new Image { ImageUrl = "img1" } }
                }
            };
            listingRepoMock.Setup(r => r.GetAllForSaleAsync()).ReturnsAsync(listings);

            var result = await service.GetAllForSaleListingViewModelsAsync();

            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual("Sale 1", result[0].Title);
            ClassicAssert.AreEqual("CityA", result[0].City);
            ClassicAssert.AreEqual("img1", result[0].Images[0]);
        }

        [Test]
        public async Task GetAllForRentListingViewModelsAsync_ShouldReturnMappedListings()
        {
            var listings = new List<Listing>
            {
                new Listing
                {
                    Id = Guid.NewGuid(),
                    Title = "Rent 1",
                    Price = 1500,
                    Description = "Desc",
                    Address = new Address { City = "CityR", Street = "StreetR", ZipCode = 2222 },
                    Category = new Category { Name = "CatR" },
                    CategoryId = Guid.NewGuid(),
                    ListingType = new ListingType { Name = "Rent" },
                    ListingTypeId = Guid.NewGuid(),
                    Images = new List<Image> { new Image { ImageUrl = "imgR1" } }
                }
            };
            listingRepoMock.Setup(r => r.GetAllForRentAsync()).ReturnsAsync(listings);

            var result = await service.GetAllForRentListingViewModelsAsync();

            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual("Rent 1", result[0].Title);
            ClassicAssert.AreEqual("CityR", result[0].City);
            ClassicAssert.AreEqual("imgR1", result[0].Images[0]);
        }

        [Test]
        public async Task GetListingViewModelByIdAsync_ShouldReturnCorrectViewModel()
        {
            var id = Guid.NewGuid();
            var listing = new Listing
            {
                Id = id,
                Title = "Detail Title",
                Price = 300000,
                Description = "Detail Desc",
                Category = new Category { Name = "CategoryName" },
                CategoryId = Guid.NewGuid(),
                ListingType = new ListingType { Name = "TypeName" },
                ListingTypeId = Guid.NewGuid(),
                Address = new Address { City = "CityD", Street = "StreetD", ZipCode = 3333 },
                Images = new List<Image>
                {
                    new Image { ImageUrl = "imgD1", IsPrimary = false },
                    new Image { ImageUrl = "imgD2", IsPrimary = true }
                }
            };

            listingRepoMock.Setup(r => r.GetListingWithDetailsAsync(id)).ReturnsAsync(listing);

            var result = await service.GetListingViewModelByIdAsync(id);

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(listing.Id, result!.Id);
            ClassicAssert.AreEqual("Detail Title", result.Title);
            ClassicAssert.AreEqual("CityD", result.City);
            ClassicAssert.AreEqual("imgD2", result.Images.First());
        }

        [Test]
        public async Task EditListingAsync_ShouldReturnFalse_WhenInputIsNull()
        {
            var result = await service.EditListingAsync(Guid.NewGuid(), null!);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task EditListingAsync_ShouldReturnFalse_WhenValidationFails()
        {
            var invalidModel = new EditListingInputModel(); // Missing required fields

            var result = await service.EditListingAsync(Guid.NewGuid(), invalidModel);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task EditListingAsync_ShouldReturnFalse_WhenListingNotFound()
        {
            var validModel = new EditListingInputModel
            {
                Title = "New Title",
                Price = 1000,
                Description = "Desc",
                CategoryId = Guid.NewGuid(),
                ListingTypeId = Guid.NewGuid(),
                City = "City",
                Street = "Street",
                ZipCode = 555,
                UploadedImagePaths = new List<string>()
            };

            listingRepoMock.Setup(r => r.GetListingWithDetailsAsync(It.IsAny<Guid>())).ReturnsAsync((Listing?)null);

            var result = await service.EditListingAsync(Guid.NewGuid(), validModel);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void GetUpdateListingInputModelByIdAsync_ShouldMapCorrectly()
        {
            var model = new ListingDetailsViewModel
            {
                Title = "Title",
                Description = "Desc",
                Price = 100,
                City = "City",
                Street = "Street",
                ZipCode = 111,
                CategoryId = Guid.NewGuid(),
                ListingTypeId = Guid.NewGuid(),
                Images = new List<string> { "img1" },
            };

            var inputModel = service.GetUpdateListingInputModelByIdAsync(model);

            ClassicAssert.NotNull(inputModel);
            ClassicAssert.AreEqual(model.Title, inputModel!.Title);
            ClassicAssert.AreEqual(model.Images.Count, inputModel.UploadedImagePaths.Count);
        }

        [Test]
        public async Task GetCreateListingInputViewModelAsync_ShouldReturnSelectLists()
        {
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "Cat1" } };
            var listingTypes = new List<ListingType> { new ListingType { Id = Guid.NewGuid(), Name = "Type1" } };
            var agents = new List<Agent> { new Agent { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" } };

            categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            listingTypeRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(listingTypes);
            agentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(agents);

            var result = await service.GetCreateListingInputViewModelAsync();

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(categories[0].Name, result.Categories.First().Text);
            ClassicAssert.AreEqual(agents[0].FirstName + " " + agents[0].LastName, result.Agents.First().Text);
        }

        [Test]
        public async Task GetCreateNewEnquiryInputViewModelAsync_ShouldReturnSelectLists()
        {
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "CatE" } };
            var listingTypes = new List<ListingType> { new ListingType { Id = Guid.NewGuid(), Name = "TypeE" } };
            var agents = new List<Agent> { new Agent { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" } };

            categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            listingTypeRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(listingTypes);
            agentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(agents);

            var result = await service.GetCreateNewEnquiryInputViewModelAsync();

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(categories[0].Name, result.Categories.First().Text);
            ClassicAssert.AreEqual(agents[0].FirstName + " " + agents[0].LastName, result.Agents.First().Text);
        }
    }
}
