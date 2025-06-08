using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations.EFCoreMappings
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Listing Category identifier (primary key)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)//class
                .HasComment("Name of the category (e.g., For Sale, For Rent)");

            entity.HasMany(e => e.Listings)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
