using Icof.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Icof.Api.Data
{
    /// <summary>
    /// Seeds the "Admin" role and a fixed set of admin accounts at startup, if they don't
    /// already exist. This is a bootstrap convenience, not a real user-management system —
    /// once there's an admin UI for managing accounts/roles, seeding hardcoded accounts here
    /// stops being the right approach. Passwords are set once on first creation only; changing
    /// them later requires the normal password-reset flow, not editing this file.
    /// </summary>
    public static class AuthSeeder
    {
        private const string AdminRole = "Admin";

        private static readonly (string Email, string Password)[] AdminAccounts =
        {
            ("pece@icof.mk", "Pece123!"),
            ("cupetre@icof.mk", "Cupetre123!"),
            ("kjupeva@icof.mk", "Kjupeva123!")
        };

        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            foreach (var (email, password) in AdminAccounts)
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };

                    var createResult = await userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to seed admin account \"{email}\": {errors}");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, AdminRole))
                {
                    await userManager.AddToRoleAsync(user, AdminRole);
                }
            }
        }
    }
}
