using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class ImageSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Images.Any())
            {
                context.Images.AddRange(
                    new Image
                    {
                        Id = Guid.Parse("dca1780c-e517-4192-b031-5212f8706f30"),
                        ImageUrl = "/images/1.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.Parse("e89946ec-c578-4754-80b6-f5669937fd1b"),
                        ImageUrl = "/images/2.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("351156bf-6cba-4bda-a5e7-3d85088d2cea")
                    },
                    new Image
                    {
                        Id = Guid.Parse("9b36d9f1-184c-4082-a9fe-b0fdf53879db"),
                        ImageUrl = "/images/3.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("9a42dc8d-1d5d-4db9-ba55-bc893135f8e7")
                    },
                    new Image
                    {
                        Id = Guid.Parse("9088757c-6b97-405e-9c88-5eabd9d8ab2d"),
                        ImageUrl = "/images/4.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("8134e79c-dde5-4a0a-841d-0c3bfc42cffd")
                    },
                    new Image
                    {
                        Id = Guid.Parse("b81bfb21-8e06-4be6-9c77-a2b1cd4eb37e"),
                        ImageUrl = "/images/5.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("22a770ab-04ff-49ce-a5b6-e9893c7ed145")
                    },
                    new Image
                    {
                        Id = Guid.Parse("41e5c7e3-cf7d-495c-a14e-fbfe4a7f860c"),
                        ImageUrl = "/images/6.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("62396a7c-4370-4052-b5fe-4edf21ae1be2")
                    },
                    new Image
                    {
                        Id = Guid.Parse("1da090b6-6e00-45c1-bcf8-35085de0265b"),
                        ImageUrl = "/images/7.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("0df25305-5059-4e7c-99e8-820d4d575e74")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/5.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/6.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/7.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/3.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/1.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/2.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/4.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/1.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/2.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = "/images/4.jpg",
                        IsPrimary = true,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
