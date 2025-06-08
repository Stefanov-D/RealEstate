using Microsoft.AspNetCore.Identity;
using RealEstate.Infrastructure.Identity;

namespace RealEstate.Infrastructure.Data.Seeding
{
    public class IdentitySeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentitySeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var roles = new[] { "Admin", "Agent", "Customer" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@example.com";
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Add more users/roles as needed...
            var agentEmail = "agent@example.com";
            if (await _userManager.FindByEmailAsync(agentEmail) == null)
            {
                var agentUser = new ApplicationUser
                {
                    UserName = agentEmail,
                    Email = agentEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(agentUser, "Agent@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(agentUser, "Agent");
            }
            //
            var customerEmail = "customer@example.com";
            if (await _userManager.FindByEmailAsync(customerEmail) == null)
            {
                var customerUser = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(customerUser, "Customer@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(customerUser, "Customer");
            }
        }
    }
}