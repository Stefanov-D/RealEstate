using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class ListingEntityConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasComment("Primary key: unique identifier for the property");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200)  //To be verified and moved to a common class
                .HasComment("Title or name of the property listing");

            entity.Property(e => e.Price)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasComment("Price of the property in local currency");

            entity.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(2024)   // to be verified and moved to a common class
                .HasComment("Optional detailed description of the property");

            entity.Property(e => e.AgentId)
                .HasComment("Foreign key to the Agent table");           

            entity.Property(e => e.AddressId)
                .IsRequired()
                .HasComment("Foreign key to the Address table");
                        
            entity.Property(e => e.CategoryId)
                .IsRequired()
                .HasComment("Foreign key to the Category table");
                    
            entity.Property(e => e.ListingTypeId)
                .IsRequired()
                .HasComment("Foreign key to the ListingType table");

            entity.Property(e => e.IsNewEnquiry)
                .HasDefaultValue(false)
                .HasComment("Indicator if the listing is new enquiry");
                
            entity.HasOne(e => e.Agent)
                .WithMany(a => a.Listings)
                .HasForeignKey(e => e.AgentId);

            entity.HasOne(p => p.Address)
                .WithOne(a => a.Listing)
                .HasForeignKey<Listing>(p => p.AddressId);

            entity.HasOne(p => p.Category)
                .WithMany(pt => pt.Listings)
                .HasForeignKey(p => p.CategoryId);

            entity.HasOne(p => p.ListingType)
                .WithMany(pt => pt.Listings)
                .HasForeignKey(p => p.ListingTypeId);

            entity.HasMany(p => p.Images)
                .WithOne(i => i.Listing)
                .HasForeignKey(p => p.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
