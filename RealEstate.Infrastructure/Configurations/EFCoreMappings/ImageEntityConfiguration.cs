using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class ImageEntityConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> entity)
        {             
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Image identifier (primary key)");

            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(255) //in class
                .HasComment("URL or path to the image file");

            entity.Property(e => e.IsPrimary)
                .IsRequired()
                .HasComment("Indicates whether the image is the main image for the property");

            entity.Property(e => e.ListingId)
                .IsRequired()
                .HasComment("Foreign key referencing the associated property");

            entity.HasOne(e => e.Listing)
                .WithMany(p => p.Images)
                .HasForeignKey(e => e.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
