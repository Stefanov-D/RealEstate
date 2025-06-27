namespace RealEstate.Application.DTOs
{
    public class ListingDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int? ZipCode { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Category { get; set; }
        public Guid? ListingTypeId { get; set; }
        public string? ListingType { get; set; }
        public Guid? AgentId { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
