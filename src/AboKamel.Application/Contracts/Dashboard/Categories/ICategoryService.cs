using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Domain.Entities.Categories;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Dashboard.Categories;

public interface ICategoryService : ICrudService<CategoryRequestDto, Category, CategoryResponseDto, CategoryDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<CategoryDetailedResponseDto>> GetCategoryBrandsAsync(int id);
}