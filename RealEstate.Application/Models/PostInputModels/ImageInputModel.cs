namespace RealEstate.Application.Models.PostInputModels
{
    public class ImageInputModel
    {
        public Guid Id = Guid.NewGuid();

        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public Guid ListingId { get; set; }
    }
}
