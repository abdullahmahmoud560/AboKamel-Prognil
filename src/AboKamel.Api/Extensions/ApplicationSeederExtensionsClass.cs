using AboKamel.Api.SeedData;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Entities.Users;
using Services.Infrastructure.DbContexts;

namespace AboKamel.Api.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<CapsulaDbContext>();
        var env = services.GetRequiredService<IWebHostEnvironment>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // تم حذف منطق إضافة ParentBrandId لأنه كان يسبب خطأ في قاعدة البيانات

        // create customer role if not exists
        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        // seed review user
        await ReviewUserSeeder.SeedReviewUserAsync(userManager);

        // seed brands first!
        await BrandSeeder.SeedBrandsAsync(context);

        // seed products
        await ProductSeeder.SeedProductsAsync(context, env);
    }
}