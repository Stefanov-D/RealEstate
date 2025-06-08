using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Agent> Agents { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<ListingType> ListingTypes { get; set; }

        public DbSet<Image> Images { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
