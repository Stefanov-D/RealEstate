namespace RealEstate.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public int? ZipCode { get; set; }

        public Guid ListingId { get; set; }

        public Listing Listing { get; set; } = null!;
    }
}
