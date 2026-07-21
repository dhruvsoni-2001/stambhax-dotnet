using Microsoft.EntityFrameworkCore;
using StambhaX.Api.Models;
using Isopoh.Cryptography.Argon2;

namespace StambhaX.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // 1. Seed Roles
        var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "System Administrator" };
        var userRole = new Role { Id = Guid.NewGuid(), Name = "User", Description = "Standard User" };

        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(adminRole, userRole);
            await context.SaveChangesAsync();
        }

        // 2. Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@stambhax.local",
                PasswordHash = Argon2.Hash("AdminPassword123!"), 
                IsActive = true
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // 3. Assign Admin Role to Admin User
            var dbAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (dbAdminRole != null)
            {
                context.UserRoles.Add(new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = dbAdminRole.Id
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
