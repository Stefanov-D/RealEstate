using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Id = Guid.Parse("92ba9297-2f3b-49b2-86ab-4ef51f508a94"), Name = "For Sale" },
                    new Category { Id = Guid.Parse("b1ae36db-455d-4099-82fd-bb5804b9aa61"), Name = "For Rent" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
