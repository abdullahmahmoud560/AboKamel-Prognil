using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Dashboard.Products;

public interface IProductService : ICrudService<ProductRequestDto, Product, ProductResponseDto, ProductDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<ProductDetailedResponseDto>> GetProductDetailsAsync(int productId);
    Task<ResultAbstract<List<ProductResponseDto>>> GetProductsWithDetailsAsync();
    Task<ResultAbstract<ProductResponseDto>> CreateProductSellingUnits(List<ProductSellingUnitRequestDto> productSellingUnits);
    Task<ResultAbstract<ProductResponseDto>> UpdateProductSellingUnit(ProductSellingUnitRequestDto request);
    Task<ResultAbstract<List<ProductResponseDto>>> GetLowStockProductsAsync(int threshold = 10);
    Task<ResultAbstract<List<ProductResponseDto>>> GetBestSellingProductsAsync(int take = 10);
    Task<ResultAbstract<int>> GetLowStockProductsCountAsync(int threshold = 10);
    Task<ResultAbstract<int>> GetBestSellingProductsCountAsync();
    Task<ResultAbstract<PagedResultDto<ProductResponseDto>>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10);
}