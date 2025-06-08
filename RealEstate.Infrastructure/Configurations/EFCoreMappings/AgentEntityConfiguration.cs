using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class AgentEntityConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Primary key for the Agent entity");

            entity.Property(e => e.FirstName)
                .HasComment("First name of the agent");

            entity.Property(e => e.LastName)
                .HasComment("Last name of the agent");

            entity.Property(e => e.Biography)
                .HasMaxLength(2000) // Optional longer text field
                .HasComment("Optional biography or profile description of the agent");

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(450) // Default length for ASP.NET Identity user id (string)
                .HasComment("Foreign key referencing the related ApplicationUser");

            entity
                 .HasOne<ApplicationUser>() // no direct property, so specify the type
                 .WithOne(u => u.AgentProfile)
                 .HasForeignKey<Agent>(a => a.UserId);

            entity.HasMany(e => e.Listings)
                .WithOne(p => p.Agent)
                .HasForeignKey(p => p.AgentId);
        }
    }
}
