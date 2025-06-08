using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Primary key for the Address entity");

            entity.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(255) // to be moved to a class
                .HasComment("Street address of the property (e.g., 123 Main St)");

            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100)// to be moved to a class
                .HasComment("City where the property is located");

            entity.Property(e => e.ZipCode)
                .IsRequired(false)
                .HasComment("Optional ZIP or postal code for the address");

            entity.Property(e => e.ListingId)
                .HasComment("Foreign key referencing the related Property");

            entity.HasOne(e => e.Listing)
                .WithOne(p => p.Address)
                .HasForeignKey<Address>(e => e.ListingId);
        }
    }
}
