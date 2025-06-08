namespace RealEstate.Domain.Entities
{
    public class Listing
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public Guid? AgentId { get; set; }
        public Agent? Agent { get; set; }

        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        // Indicates the category of the listing (e.g., Residential, Business)
        public Guid? ListingTypeId { get; set; }
        public ListingType? ListingType { get; set; }

        // Indicates the type of property or business (e.g., Apartment, Shop, Office)
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;

        public ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
