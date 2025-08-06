using RealEstate.Application.Interfaces;
using RealEstate.Application.Services;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure;
using RealEstate.Infrastructure.Data.Seeding.ExtensionMethods;
using RealEstate.Infrastructure.Repository;
using RealEstate.Infrastructure.Services;

namespace RealEstate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IListingRepository, ListingRepository>();
            builder.Services.AddScoped<IAgentRepository, AgentRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IListingTypeRepository, ListingTypeRepository>();
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
            builder.Services.AddScoped<IListingService, ListingService>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();   
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();

            builder.Services.AddRazorPages();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.SeedDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");            

            app.MapRazorPages();

            app.Run();
        }
    }
}
