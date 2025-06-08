using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Services;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IListingService, ListingService>();

            return services;
        }
    }
}
