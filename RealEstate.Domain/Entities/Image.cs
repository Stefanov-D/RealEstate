namespace RealEstate.Domain.Entities
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public Guid ListingId { get; set; }

        public Listing Listing { get; set; } = null!;
    }
}
