using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address, Guid>, IAsyncGenericRepository<Address, Guid>
    {
    }
}
