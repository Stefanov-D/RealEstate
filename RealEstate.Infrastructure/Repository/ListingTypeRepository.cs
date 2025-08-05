using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repository
{
    public class ListingTypeRepository : BaseRepository<ListingType, Guid>, IListingTypeRepository
    {
        public ListingTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
