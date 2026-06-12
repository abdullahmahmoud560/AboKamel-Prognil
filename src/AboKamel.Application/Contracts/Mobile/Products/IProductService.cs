using Capsula.Application.Dtos.Mobile.Products;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Products;

public interface IProductService : IApplicationService, IScopedService
{
    Task<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>> GetNewestProductsAsync();
    Task<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>> SearchProductsAsync(ProductSearchRequestDto searchTerm, string? userId = null);
    Task<ResultAbstract<List<ProductFavoriteResponseDto>>> GetAllProductsWithFavoriteAsync(string? userId = null);
}