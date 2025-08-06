namespace RealEstate.Infrastructure.Data.Seeding
{
    public interface ISeedManager
    {
        public Task SeedAllAsync(ApplicationDbContext context);
    }
}
