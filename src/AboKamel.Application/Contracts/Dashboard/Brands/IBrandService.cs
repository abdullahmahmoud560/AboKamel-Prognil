using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;
using Services.Core.DependencyInjection;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Dashboard.Brands;

public interface IBrandService : ICrudService<BrandRequestDto, Brand, BrandResponseDto, BrandDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<BrandDetailedResponseDto>> GetBrandWithProducts(int id);
    Task<ResultAbstract<PagedResultDto<BrandResponseDto>>> GetAllBrandsAsync(int pageNumber = 1, int pageSize = 10);
}
