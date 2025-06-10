using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class ListingSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Listings.Any())
            {
                context.Listings.AddRange(
                    new Listing
                    {
                        Id = Guid.Parse("c45ea8e7-6dc7-4be8-a51a-163597b64ce3"),
                        Title = "Property 1",
                        Price = 105000,
                        Description = "Description for property 1",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("bd81c3f5-e931-470c-8f1d-738aca6875bb"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("DFC7EEB8-1567-40B6-99B3-020F59883226"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("351156bf-6cba-4bda-a5e7-3d85088d2cea"),
                        Title = "Property 2",
                        Price = 110000,
                        Description = "Description for property 2",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("a6ba07e7-5b18-4ddb-b23f-a15e35eb10df"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("DFC7EEB8-1567-40B6-99B3-020F59883226"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("9a42dc8d-1d5d-4db9-ba55-bc893135f8e7"),
                        Title = "Property 3",
                        Price = 115000,
                        Description = "Description for property 3",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("a0ad82a3-cc6c-4c88-88c3-75b65e4bf064"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("DFC7EEB8-1567-40B6-99B3-020F59883226"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("8134e79c-dde5-4a0a-841d-0c3bfc42cffd"),
                        Title = "Property 4",
                        Price = 120000,
                        Description = "Description for property 4",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("3efd2e0b-6af1-4fb5-aa89-a79ea8948268"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("DFC7EEB8-1567-40B6-99B3-020F59883226"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("22a770ab-04ff-49ce-a5b6-e9893c7ed145"),
                        Title = "Property 5",
                        Price = 125000,
                        Description = "Description for property 5",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("5a2cae53-e1ac-4e8c-abf1-66a840ee8d5b"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("DFC7EEB8-1567-40B6-99B3-020F59883226"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("62396a7c-4370-4052-b5fe-4edf21ae1be2"),
                        Title = "Property 6",
                        Price = 130000,
                        Description = "Description for property 6",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("4b65e413-0798-4f84-a201-300d4b864d15"),
                        CategoryId = Guid.Parse("92BA9297-2F3B-49B2-86AB-4EF51F508A94"),
                        ListingTypeId = Guid.Parse("7D0D777C-5883-4A04-ADAC-1E3716D4E362"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("0df25305-5059-4e7c-99e8-820d4d575e74"),
                        Title = "Property 7",
                        Price = 135000,
                        Description = "Description for property 7",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("7188fe77-d8c3-4b7a-9b90-c2c4b38e85e7"),
                        CategoryId = Guid.Parse("B1AE36DB-455D-4099-82FD-BB5804B9AA61"),
                        ListingTypeId = Guid.Parse("7D0D777C-5883-4A04-ADAC-1E3716D4E362"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("d33f47ed-f524-452e-981a-6e91e74686e8"),
                        Title = "Property 8",
                        Price = 140000,
                        Description = "Description for property 8",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("55c0c7da-c0db-4318-b378-9c1d314e5fcd"),
                        CategoryId = Guid.Parse("B1AE36DB-455D-4099-82FD-BB5804B9AA61"),
                        ListingTypeId = Guid.Parse("7D0D777C-5883-4A04-ADAC-1E3716D4E362"),
                        IsNewEnquiry = false
                    },
                    new Listing
                    {
                        Id = Guid.Parse("fa7f32b3-9cdb-4b37-9896-fb2b36d67e3a"),
                        Title = "Property 9",
                        Price = 145000,
                        Description = "Description for property 9",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("850e55af-c748-477c-9bba-991972fa0e89"),
                        CategoryId = Guid.Parse("B1AE36DB-455D-4099-82FD-BB5804B9AA61"),
                        ListingTypeId = Guid.Parse("7D0D777C-5883-4A04-ADAC-1E3716D4E362"),
                        IsNewEnquiry = true
                    },
                    new Listing
                    {
                        Id = Guid.Parse("47bb1fef-3bf9-4d08-ba3c-0358a9b69989"),
                        Title = "Property 10",
                        Price = 150000,
                        Description = "Description for property 10",
                        AgentId = Guid.Parse("DE942F9A-7E65-4A3B-8C78-D299E2A95FD1"),
                        AddressId = Guid.Parse("b4a2e294-df02-4cdb-8bf6-e0a8bf3bbefe"),
                        CategoryId = Guid.Parse("B1AE36DB-455D-4099-82FD-BB5804B9AA61"),
                        ListingTypeId = Guid.Parse("7D0D777C-5883-4A04-ADAC-1E3716D4E362"),
                        IsNewEnquiry = true
                    }
                );


                await context.SaveChangesAsync();
            }
        }
    }
}
