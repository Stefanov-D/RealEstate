using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IAgentRepository : IGenericRepository<Agent, Guid>, IAsyncGenericRepository<Agent, Guid>
    {
    }
}
