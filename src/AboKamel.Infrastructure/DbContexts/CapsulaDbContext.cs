using AboKamel.Domain.Entities.Advertisements;
using AboKamel.Domain.Entities.Areas;
using AboKamel.Domain.Entities.Debts;
using AboKamel.Domain.Entities.Notificationns;
using AboKamel.Domain.Entities.Offers;
using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Domain.Entities.Addresses;
using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Entities.Categories;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Prescriptions;
using Capsula.Domain.Entities.Products;
using Capsula.Domain.Entities.Supports;
using Capsula.Domain.Entities.Users.Customers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities.Users;
using System.Reflection;
using System.Reflection.Emit;

namespace Services.Infrastructure.DbContexts;
/// <summary>
/// Represents the Services database context.
/// </summary>
public class CapsulaDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the ServicesDbContext class.
    /// </summary>
    /// <param name="options">The ServicesDbContext to use.</param>
    public CapsulaDbContext(DbContextOptions options) : base(options)
    {
    }

    #region Entities
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionImage> PrescriptionImages { get; set; }
    public DbSet<VoiceRecord> VoiceRecords { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Support> Supports { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<SellingUnit> SellingUnits { get; set; }
    public DbSet<Debt> Debts { get; set; }
    public DbSet<ProductSellingUnit> ProductSellingUnits { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<TwoFactorVerify> TwoFactorVerifies { get; set; }
    #endregion

    /// <summary>
    /// Configures the model by applying configurations from the current assembly.
    /// </summary>
    /// <param name="builder">The ModelBuilder to use.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Call the base method to perform any additional configuration
        base.OnModelCreating(builder);

        builder.Entity<CartItem>()
        .HasOne(ci => ci.Cart)
        .WithMany(c => c.Items)
        .HasForeignKey(ci => ci.CartId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
        .HasOne(oi => oi.Order)
        .WithMany(o => o.Items)
        .HasForeignKey(oi => oi.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Address>()
        .HasOne(a => a.Customer)
        .WithMany(c => c.Addresses)
        .HasForeignKey(a => a.CustomerId)
        .OnDelete(DeleteBehavior.Cascade);
    }

}
