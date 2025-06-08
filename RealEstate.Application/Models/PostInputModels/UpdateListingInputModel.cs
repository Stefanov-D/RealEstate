using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Models.PostInputModels
{
    public class UpdateListingInputModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters.")]
        [MaxLength(50, ErrorMessage = "Title can't exceed 50 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Price is required.")]
        [Precision(18, 2)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100, ErrorMessage = "City name can't exceed 100 characters.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(100, ErrorMessage = "Street name can't exceed 100 characters.")]
        public string Street { get; set; } = null!;

        /*[Range(10000, 99999, ErrorMessage = "Zip code must be a 5-digit number.")]*/
        public int? ZipCode { get; set; }

        [MaxLength(2000, ErrorMessage = "Description can't exceed 2000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Guid? CategoryId { get; set; }

        public string? Category { get; set; }

        [Required(ErrorMessage = "Listing type is required.")]
        public Guid? ListingTypeId { get; set; }

        [DisplayName("Type")]
        public string? ListingType { get; set; }

        public List<string> UploadedImagePaths { get; set; } = new List<string>();
    }
}

