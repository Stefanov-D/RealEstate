using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RealEstate.Infrastructure.Data.Seeding.ExtensionMethods
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var seedManager = services.GetRequiredService<ISeedManager>();
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    seedManager.SeedAllAsync(dbContext).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILoggerFactory>()
                     .CreateLogger("DbSeeder");
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
