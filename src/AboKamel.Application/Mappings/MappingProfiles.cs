using AboKamel.Application.Mappings.Dashboard.Areas;
using AboKamel.Application.Mappings.Dashboard.Debts;
using AboKamel.Application.Mappings.Dashboard.Notifications;
using AboKamel.Application.Mappings.Dashboard.Offers;
using AboKamel.Application.Mappings.Dashboard.Orders;
using AboKamel.Application.Mappings.Dashboard.SellingUnits;
using AboKamel.Application.Mappings.Mobile.Orders;
using AutoMapper;
using Capsula.Application.Mappings.Dashboard.Brands;
using Capsula.Application.Mappings.Dashboard.Categories;
using Capsula.Application.Mappings.Dashboard.Products;
using Capsula.Application.Mappings.Mobile.Carts;
using Capsula.Application.Mappings.Mobile.Products;
using Capsula.Application.Mappings.Mobile.Supports;

namespace Services.Application.Mappings;
/// <summary>
/// A class representing a collection of mapping profiles.
/// </summary>
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMappings();
    }

    /// <summary>
    /// Registers all entity and DTO mappings by calling the respective extension methods.
    /// </summary>
    private void CreateMappings()
    {
        this.AddAuthMapping();
        this.AddBrandMapping();
        this.AddCategoryMapping();
        this.AddProductMapping();
        this.AddCartItemMapping();
        this.AddCartMapping();
        this.AddAddressMapping();
        this.AddFavoriteMapping();
        this.AddSupportMapping();
        this.AddAreaMapping();
        this.AddOfferMapping();
        this.AddSellingUnitMapping();
        this.AddProductSellingUnitMapping();
        this.AddOrderDashboardMapping();
        this.AddOrderMapping();
        this.AddDebtMapping();
        this.AddNotificationMapping();
    }

}
