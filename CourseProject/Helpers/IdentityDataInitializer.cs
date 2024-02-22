using CourseProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Helpers;

public class IdentityDataInitializer
{
    public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }

    public static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("менеджер"))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "менеджер" });
        }
        if (!await roleManager.RoleExistsAsync("клієнт"))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "клієнт" });
        }
    }

    public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.FindByNameAsync("manager@example.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "manager@example.com",
                Email = "manager@example.com"
            };
            var result = await userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "менеджер");
            }
        }

        if (await userManager.FindByNameAsync("client@example.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "client@example.com",
                Email = "client@example.com"
            };
            var result = await userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "клієнт");
            }
        }
    }
}