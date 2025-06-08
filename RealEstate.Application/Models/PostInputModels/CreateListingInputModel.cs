using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Models.PostInputModels
{
    public class CreateListingInputModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Street { get; set; } = null!;

        public int? ZipCode { get; set; }

        [MaxLength(600)]
        public string? Description { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public string? Category { get; set; }

        [Required]
        public Guid ListingTypeId { get; set; }

        [DisplayName("Type")]
        public string? ListingType { get; set; }

        public List<string> UploadedImagePaths { get; set; } = new List<string>();
    }
}

