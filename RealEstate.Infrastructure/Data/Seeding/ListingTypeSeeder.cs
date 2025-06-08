using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class ListingTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.ListingTypes.Any())
            {
                context.ListingTypes.AddRange(
                    new ListingType { Id = Guid.Parse("7d0d777c-5883-4a04-adac-1e3716d4e362"), Name = "Apartment" },
                    new ListingType { Id = Guid.Parse("dfc7eeb8-1567-40b6-99b3-020f59883226"), Name = "House" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
