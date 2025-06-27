using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Identity;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Primary key for the Customer entity");

            entity.Property(e => e.FirstName)
                .HasComment("First name of the customer");

            entity.Property(e => e.LastName)
                .HasComment("Last name of the customer");

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(450)
                .HasComment("Foreign key referencing the related ApplicationUser");

            entity
                 .HasOne<ApplicationUser>()
                 .WithOne(u => u.CustomerProfile)
                 .HasForeignKey<Customer>(a => a.UserId)
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
