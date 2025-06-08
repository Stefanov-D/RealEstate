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
    public class ListingTypeEntityConfiguration : IEntityTypeConfiguration<ListingType>
    {
        public void Configure(EntityTypeBuilder<ListingType> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Listing type identifier (primary key)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100) // class
                .HasComment("Name of the listing type (e.g., For Sale, For Rent)");

            entity.HasMany(e => e.Listings)
                .WithOne(p => p.ListingType)
                .HasForeignKey(p => p.ListingTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
