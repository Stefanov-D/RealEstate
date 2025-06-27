using Microsoft.AspNetCore.Identity;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Agent? AgentProfile { get; set; } = null!;

        public Customer? CustomerProfile { get; set; } = null!;
    }
}
