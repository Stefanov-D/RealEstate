using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repository
{
    public class AgentRepository : BaseRepository<Agent, Guid>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
