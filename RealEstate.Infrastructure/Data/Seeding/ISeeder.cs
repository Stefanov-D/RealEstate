using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext context);
    }
}
