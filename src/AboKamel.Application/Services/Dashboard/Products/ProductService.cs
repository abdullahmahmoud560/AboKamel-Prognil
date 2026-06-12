using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using AutoMapper;
using Capsula.Application.Contracts.Dashboard.Products;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.Core.Dtos;
using Services.Core.Results;
using Services.Infrastructure.DbContexts;

namespace Capsula.Application.Services.Dashboard.Products;

public class ProductService : CrudService<ProductRequestDto, Product, Dtos.Dashboard.Products.ProductResponseDto, ProductDetailedResponseDto, int>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Product> _logger;
    private readonly IImageService _imageService;
    private readonly IMemoryCache _memoryCache;
    private const string ProductCachePrefix = "products_page_";


    public ProductService(IProductRepository repository, IMapper mapper, ILogger<Product> logger, IImageService imageService, IMemoryCache memoryCache) : base(repository, mapper, logger)
    {
        _productRepository = repository;
        _mapper = mapper;
        _logger = logger;
        _imageService = imageService;
        _memoryCache = memoryCache;
    }

    public async Task<ResultAbstract<ProductDetailedResponseDto>> GetProductDetailsAsync(int productId)
    {
        var products = await _productRepository.GetProductDetailsAsync(productId);
        return Result.Success(_mapper.Map<ProductDetailedResponseDto>(products));
    }

    public async Task<ResultAbstract<Dtos.Dashboard.Products.ProductResponseDto>> CreateProductSellingUnits(List<ProductSellingUnitRequestDto> request)
    {
        var product = await _productRepository.GetByIdAsync(request.First().ProductId);

        if (product == null)
        {
            return Result.Error("لم يتم العثور على المنتج.");
        }

        var productSellingUnits = _mapper.Map<List<ProductSellingUnit>>(request);

        var dbContext = (CapsulaDbContext)_productRepository.GetDbContext();

        await dbContext.AddRangeAsync(productSellingUnits);
        await dbContext.SaveChangesAsync();

        var productResponse = _mapper.Map<Dtos.Dashboard.Products.ProductResponseDto>(product);

        return Result.Success(productResponse);
    }

    public async Task<ResultAbstract<Dtos.Dashboard.Products.ProductResponseDto>> UpdateProductSellingUnit(ProductSellingUnitRequestDto request)
    {
        var dbContext = (CapsulaDbContext)_productRepository.GetDbContext();
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        var productSellingUnit = await dbContext.ProductSellingUnits.Where(p => p.ProductId == request.ProductId && p.SellingUnitId == request.SellingUnitId).SingleOrDefaultAsync();

        if (productSellingUnit == null)
        {
            return Result.Error("لم يتم العثور على وحدة بيع المنتج.");
        }

        //var productSellingUnits = _mapper.Map<ProductSellingUnit>(request);
        
        productSellingUnit.Quantity = request.Quantity;
        productSellingUnit.Price = request.Price;

        await dbContext.SaveChangesAsync();

        var productResponse = _mapper.Map<Dtos.Dashboard.Products.ProductResponseDto>(product);

        return Result.Success(productResponse);
    }

    public async Task<ResultAbstract<List<Dtos.Dashboard.Products.ProductResponseDto>>> GetProductsWithDetailsAsync()
    {
        var products = await _productRepository.GetProductsWithDetailsAsync();
        return Result.Success(_mapper.Map<List<Dtos.Dashboard.Products.ProductResponseDto>>(products));
    }

    public async Task<ResultAbstract<List<Dtos.Dashboard.Products.ProductResponseDto>>> GetLowStockProductsAsync(int threshold = 10)
    {
        var products = await _productRepository.GetLowStockProductsAsync(threshold);

        if (products == null || products.Count == 0)
        {
            return Result.Error("لم يتم العثور على منتجات منخفضة المخزون.");
        }

        var response = _mapper.Map<List<Dtos.Dashboard.Products.ProductResponseDto>>(products);

        return Result.Success(response);
    }

    public async Task<ResultAbstract<List<Dtos.Dashboard.Products.ProductResponseDto>>> GetBestSellingProductsAsync(int take = 10)
    {
        var products = await _productRepository.GetBestSellingProductsAsync(take);

        if (products == null || products.Count == 0)
        {
            return Result.Error("لم يتم العثور على منتجات الأكثر مبيعاً.");
        }

        var response = _mapper.Map<List<Dtos.Dashboard.Products.ProductResponseDto>>(products);

        return Result.Success(response);
    }

    public async Task<ResultAbstract<int>> GetLowStockProductsCountAsync(int threshold = 10)
    {
        var count = await _productRepository.GetLowStockProductsCountAsync(threshold);
        return Result.Success(count);
    }

    public async Task<ResultAbstract<int>> GetBestSellingProductsCountAsync()
    {
        var count = await _productRepository.GetBestSellingProductsCountAsync();
        return Result.Success(count);
    }

    public async Task<ResultAbstract<PagedResultDto<ProductResponseDto>>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var cacheKey = $"{ProductCachePrefix}{pageNumber}_size_{pageSize}";

        if (_memoryCache.TryGetValue(cacheKey, out ResultAbstract<PagedResultDto<ProductResponseDto>>? cachedResult))
        {
            return cachedResult!;
        }

        var (products, totalCount) = await _productRepository.GetPagedProductsAsync(pageNumber, pageSize);

        var response = new PagedResultDto<ProductResponseDto>(totalCount, _mapper.Map<List<ProductResponseDto>>(products));
        var result = Result.Success(response);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(cacheKey, result, cacheEntryOptions);

        return result;
    }

    private void ClearProductCache()
    {
        var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var cacheEntriesCollection = cacheEntriesCollectionDefinition?.GetValue(_memoryCache) as dynamic;

        if (cacheEntriesCollection == null) return;

        foreach (var cacheEntry in cacheEntriesCollection)
        {
            var cacheKey = cacheEntry.GetType().GetProperty("Key")?.GetValue(cacheEntry) as string;
            if (cacheKey != null && cacheKey.StartsWith(ProductCachePrefix))
            {
                _memoryCache.Remove(cacheKey);
            }
        }
    }

    public override async Task<ResultAbstract<ProductResponseDto>> UpdateAsync(ProductRequestDto request, int id)
    {
        var result = await base.UpdateAsync(request, id);
        ClearProductCache();
        return result;
    }

    public override async Task<ResultAbstract<ProductResponseDto>> DeleteAsync(int id)
    {
        var result = await base.DeleteAsync(id);
        ClearProductCache();
        return result;
    }
}
