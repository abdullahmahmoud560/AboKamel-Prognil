using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Dashboard.Brands;

public interface IBrandService : ICrudService<BrandRequestDto, Brand, BrandResponseDto, BrandDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<BrandDetailedResponseDto>> GetBrandWithProducts(int id);
}
