using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class AddressSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Addresses.Any())
            {
                context.Addresses.AddRange(
                    new Address
                    {
                        Id = Guid.Parse("bd81c3f5-e931-470c-8f1d-738aca6875bb"),
                        Street = "1 Main St",
                        City = "City1",
                        ZipCode = 10001,
                        ListingId = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3")
                    },
                    new Address
                    {
                        Id = Guid.Parse("a6ba07e7-5b18-4ddb-b23f-a15e35eb10df"),
                        Street = "2 Main St",
                        City = "City2",
                        ZipCode = 10002,
                        ListingId = Guid.Parse("351156bf-6cba-4bda-a5e7-3d85088d2cea")
                    },
                    new Address
                    {
                        Id = Guid.Parse("a0ad82a3-cc6c-4c88-88c3-75b65e4bf064"),
                        Street = "3 Main St",
                        City = "City3",
                        ZipCode = 10003,
                        ListingId = Guid.Parse("9a42dc8d-1d5d-4db9-ba55-bc893135f8e7")
                    },
                    new Address
                    {
                        Id = Guid.Parse("3efd2e0b-6af1-4fb5-aa89-a79ea8948268"),
                        Street = "4 Main St",
                        City = "City4",
                        ZipCode = 10004,
                        ListingId = Guid.Parse("8134e79c-dde5-4a0a-841d-0c3bfc42cffd")
                    },
                    new Address
                    {
                        Id = Guid.Parse("5a2cae53-e1ac-4e8c-abf1-66a840ee8d5b"),
                        Street = "5 Main St",
                        City = "City5",
                        ZipCode = 10005,
                        ListingId = Guid.Parse("22a770ab-04ff-49ce-a5b6-e9893c7ed145")
                    },
                    new Address
                    {
                        Id = Guid.Parse("4b65e413-0798-4f84-a201-300d4b864d15"),
                        Street = "6 Main St",
                        City = "City6",
                        ZipCode = 10006,
                        ListingId = Guid.Parse("62396a7c-4370-4052-b5fe-4edf21ae1be2")
                    },
                    new Address
                    {
                        Id = Guid.Parse("7188fe77-d8c3-4b7a-9b90-c2c4b38e85e7"),
                        Street = "7 Main St",
                        City = "City7",
                        ZipCode = 10007,
                        ListingId = Guid.Parse("0df25305-5059-4e7c-99e8-820d4d575e74")
                    },
                    new Address
                    {
                        Id = Guid.Parse("55c0c7da-c0db-4318-b378-9c1d314e5fcd"),
                        Street = "8 Main St",
                        City = "City8",
                        ZipCode = 10008,
                        ListingId = Guid.Parse("d33f47ed-f524-452e-981a-6e91e74686e8")
                    },
                    new Address
                    {
                        Id = Guid.Parse("850e55af-c748-477c-9bba-991972fa0e89"),
                        Street = "9 Main St",
                        City = "City9",
                        ZipCode = 10009,
                        ListingId = Guid.Parse("fa7f32b3-9cdb-4b37-9896-fb2b36d67e3a")
                    },
                    new Address
                    {
                        Id = Guid.Parse("b4a2e294-df02-4cdb-8bf6-e0a8bf3bbefe"),
                        Street = "10 Main St",
                        City = "City10",
                        ZipCode = 10010,
                        ListingId = Guid.Parse("47bb1fef-3bf9-4d08-ba3c-0358a9b69989")
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
