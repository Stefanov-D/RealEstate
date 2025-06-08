namespace RealEstate.Domain.Entities
{
    public class Agent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Biography { get; set; }

        public string UserId { get; set; } = null!;

        public ICollection<Listing> Listings { get; set; } = new HashSet<Listing>();
    }
}
