using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class AgentSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == "agent@example.com");

            if (!context.Agents.Any())
            {
               await context.Agents.AddRangeAsync(
                    new Agent
                    {
                        Id = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        FirstName = "Agent",
                        LastName = "007",
                        Biography = "Experienced agent",
                        UserId = user.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
