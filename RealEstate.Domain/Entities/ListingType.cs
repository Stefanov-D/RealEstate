namespace RealEstate.Domain.Entities
{
    public class ListingType
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Listing> Listings { get; set; } = new HashSet<Listing>();
    }
}
