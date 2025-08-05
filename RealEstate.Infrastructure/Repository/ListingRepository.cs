using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repository
{
    public class ListingRepository : BaseRepository<Listing, Guid>, IListingRepository
    {
        private readonly ApplicationDbContext db;

        public ListingRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Listing>> GetAllForSaleAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.ListingType)
                .Include(x => x.Address)
                .Where(x => x.Category!.Name == "For Sale" && x.IsNewEnquiry == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetAllForRentAsync()
        {
            return await db.Listings
                .AsNoTracking()
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.ListingType)
                .Include(x => x.Address)
                .Where(x => x.Category.Name == "For Rent" && x.IsNewEnquiry == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetAllNewEnquiriesAsync()
        {
            return await db.Listings
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.ListingType)
                .Include(x => x.Address)
                .Where(x => x.IsNewEnquiry == true)
                .ToListAsync();
        }

        public async Task<Listing?> GetListingWithDetailsAsync(Guid id)
        {
            return await db.Listings
                .Include(l => l.Address)
                .Include(l => l.ListingType)
                .Include(l => l.Category)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> SyncImageRecordsInDb(Guid listingId, List<string> resolvedImagePaths)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                // Reload fresh listing and images within the transaction
                var listing = await db.Listings
                    .Include(l => l.Images)
                    .FirstOrDefaultAsync(l => l.Id == listingId);

                if (listing == null)
                    return false;

                var normalizedPaths = resolvedImagePaths.Select(p => p.Trim()).ToList();

                // Remove images NOT in the updated list
                var imagesToRemove = listing.Images
                    .Where(img => !normalizedPaths.Contains(img.ImageUrl))
                    .ToList();

                foreach (var image in imagesToRemove)
                {
                    if (db.Entry(image).State == EntityState.Detached)
                    {
                        db.Images.Attach(image);
                    }
                    db.Images.Remove(image);
                }

                // Refresh images collection after removals
                await db.SaveChangesAsync();

                // Refresh listing.Images collection after removals
                await db.Entry(listing).Collection(l => l.Images).LoadAsync();

                var existingUrls = listing.Images.Select(img => img.ImageUrl).ToHashSet();

                // Add new images
                var newUrls = normalizedPaths.Where(url => !existingUrls.Contains(url)).ToList();

                foreach (var url in newUrls)
                {
                    listing.Images.Add(new Image
                    {
                        ListingId = listing.Id,
                        ImageUrl = url,
                        IsPrimary = false
                    });
                }

                // Reset all IsPrimary flags
                foreach (var image in listing.Images)
                {
                    image.IsPrimary = false;
                }

                // Set IsPrimary for first image in resolved paths
                var firstImage = listing.Images.FirstOrDefault(img => normalizedPaths.Contains(img.ImageUrl));
                if (firstImage != null)
                    firstImage.IsPrimary = true;

                await db.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Concurrency conflict while syncing images: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error syncing images: {ex.Message}");
                return false;
            }
        }



    }
}