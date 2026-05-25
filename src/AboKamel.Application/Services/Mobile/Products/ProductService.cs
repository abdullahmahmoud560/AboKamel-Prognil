using AutoMapper;
using Capsula.Application.Contracts.Mobile.Products;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Mobile.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Core.Results;
using Services.Infrastructure.DbContexts;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Application.Dtos.Dashboard.Categories;
using AboKamel.Application.Dtos.Dashboard.Areas;
using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using Capsula.Domain.Entities.Users.Customers;
using Capsula.Application.Dtos.Dashboard.Products;

namespace Capsula.Application.Services.Mobile.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductResponseDto> _logger;
    private readonly IImageService _imageService;


    public ProductService(IProductRepository repository, IMapper mapper, ILogger<ProductResponseDto> logger, IImageService imageService)
    {
        _productRepository = repository;
        _mapper = mapper;
        _logger = logger;
        _imageService = imageService;
    }

    public async Task<ResultAbstract<List<ProductFavoriteResponseDto>>> GetAllProductsWithFavoriteAsync(string userId)
    {
        CapsulaDbContext dbContext = (CapsulaDbContext)_productRepository.GetDbContext();

        Customer? customer = null;
        bool hasUser = !string.IsNullOrWhiteSpace(userId);

        if (hasUser)
        {
            customer = await dbContext.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == userId);
        }

        var query = dbContext.Products.AsQueryable();

        // ✅ Area validation
        if (customer != null)
        {
            query = query.Where(p => p.AreaId == customer.AreaId || p.AreaId == null);
        }
        else
        {
            query = query.Where(p => p.AreaId == null);
        }

        var products = await query
            .Select(p => new ProductFavoriteResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.ProductSellingUnits.FirstOrDefault().Price,
                ImageUrl = _imageService.GenerateUrl(p.ImagePath),
                BrandId = p.BrandId,
                BrandName = p.Brand.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsFavorite = hasUser && p.Favorites.Any(f => f.UserId == userId)
            })
            .ToListAsync();

        return Result.Success(products);
    }

    public async Task<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>> GetNewestProductsAsync()
    {
        var products = await _productRepository.GetNewestProductsWithDetailsAsync();
        return Result.Success(_mapper.Map<List<ProductFavoriteDetailedResponseDto>>(products));
    }

    public async Task<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>> SearchProductsAsync(Dtos.Mobile.Products.ProductSearchRequestDto searchTerm, string userId)
    {
        CapsulaDbContext dbContext = (CapsulaDbContext)_productRepository.GetDbContext();
        var query = dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm.ProductName))
        {
            query = query.Where(p => p.Name.Contains(searchTerm.ProductName));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm.ProductBrand))
        {
            query = query.Where(p => p.Brand.Name.Contains(searchTerm.ProductBrand));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm.ProductCategory))
        {
            query = query.Where(p => p.Category.Name.Contains(searchTerm.ProductCategory));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm.ProductDescription))
        {
            query = query.Where(p => p.Description.Contains(searchTerm.ProductDescription));
        }

        bool hasUser = !string.IsNullOrWhiteSpace(userId);
        Customer customer;
        if (hasUser)
        {
            customer = await dbContext.Customers.SingleOrDefaultAsync(u => u.Id == userId);

            if (customer != null)
            {
                query = query.Where(p => p.AreaId == customer.AreaId || p.AreaId == null);
            }
        }

        var products = await query.Select(p => new ProductFavoriteDetailedResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = _imageService.GenerateUrl(p.ImagePath),
            MinimumQuantityPerOrder = p.MinimumQuantityPerOrder,
            MaximumQuantityPerOrder = p.MaximumQuantityPerOrder,
            BrandId = p.BrandId,
            Brand = new BrandResponseDto
            {
                Id = p.BrandId,
                Name = p.Brand.Name
            },
            Category = new CategoryResponseDto
            {
                Id= p.CategoryId,
                Name = p.Category.Name,
            },
            Area = p.AreaId == null
            ? null
            : new AreaResponseDto
            {
                Id = p.AreaId.Value,
                Name = p.Area.Name
            },
            CategoryId = p.CategoryId,
            AreaId = p.AreaId,
            ProductSellingUnits = p.ProductSellingUnits.Select(s => new ProductSellingUnitResponseDto
            {
                Id = s.Id,
                Price = s.Price,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                SellingUnitName = s.SellingUnit.Name
                
            }).ToList(),
            IsFavorite = hasUser && p.Favorites.Any(f => f.UserId == userId)
        }).Take(12).ToListAsync();

        return Result.Success(products);
    }
}
