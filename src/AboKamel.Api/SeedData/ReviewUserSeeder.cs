using Microsoft.AspNetCore.Identity;
using Services.Domain.Entities.Users;

namespace AboKamel.Api.SeedData;

public static class ReviewUserSeeder
{
    public static async Task SeedReviewUserAsync(
        UserManager<ApplicationUser> userManager)
    {
        var email = "review456@test.com";

        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return;

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(
            user,
            "Review@456");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                user,
                "Customer");
        }
    }
}
