using RealEstate.Infrastructure.Data.Seeding;

namespace RealEstate.Infrastructure.Data.Seed
{
    public class SeedManager : ISeedManager
    {
        private readonly IEnumerable<ISeeder> _seeders;
        private readonly ApplicationDbContext _db;

        public SeedManager(ApplicationDbContext context, IEnumerable<ISeeder> seeders)
        {
            _db = context;
            _seeders = seeders;
        }

        public async Task SeedAllAsync(ApplicationDbContext context)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(_db);
            }
        }
    }
}
