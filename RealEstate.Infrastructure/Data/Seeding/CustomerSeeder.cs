using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class CustomerSeeder: ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == "customer@example.com");

            if (!context.Customers.Any())
            {
                await context.Customers.AddRangeAsync(
                    new Customer
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Customer",
                        LastName = "0078",
                        UserId = user.Id
                    });

                await context.SaveChangesAsync();
            }
        }
    }
}
