using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Data.Seed;
using RealEstate.Infrastructure.Data.Seeding;
using RealEstate.Infrastructure.Identity;


namespace RealEstate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            services.AddTransient<ISeeder, AddressSeeder>();
            services.AddTransient<ISeeder, IdentitySeeder>();
            services.AddTransient<ISeeder, AgentSeeder>();
            services.AddTransient<ISeeder, CustomerSeeder>();
            services.AddTransient<ISeeder, ListingTypeSeeder>();
            services.AddTransient<ISeeder, CategorySeeder>();
            services.AddTransient<ISeeder, ListingSeeder>();
            services.AddTransient<ISeeder, ImageSeeder>();
            services.AddScoped<ISeedManager, SeedManager>();

            return services;
        }
    }
}
