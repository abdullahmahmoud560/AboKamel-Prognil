using AutoMapper;
using Capsula.Application.Contracts.Dashboard.Brands;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Repositories.Brands;
using Microsoft.Extensions.Logging;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Application.Services.Dashboard.Brands;

public class BrandService : CrudService<BrandRequestDto, Brand, BrandResponseDto, BrandDetailedResponseDto, int>, IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Brand> _logger;

    public BrandService(IBrandRepository repository, IMapper mapper, ILogger<Brand> logger) : base(repository, mapper, logger)
    {
        _brandRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<BrandDetailedResponseDto>> GetBrandWithProducts(int id)
    {
        var brand = await _brandRepository.GetBrandWithProductsAsync(id);

        if (brand is null)
        {
            _logger.LogError("record was not found");
            return Result.Error("Brand was not found");
        }

        return Result.Success(_mapper.Map<BrandDetailedResponseDto>(brand));
    }

    public async Task<ResultAbstract<PagedResultDto<BrandResponseDto>>> GetAllBrandsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var (brands, totalCount) = await _brandRepository.GetPagedBrandsAsync(pageNumber, pageSize);
        var response = new PagedResultDto<BrandResponseDto>(totalCount, _mapper.Map<List<BrandResponseDto>>(brands));
        return Result.Success(response);
    }
}