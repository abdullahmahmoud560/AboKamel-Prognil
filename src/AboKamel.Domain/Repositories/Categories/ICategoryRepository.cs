using Capsula.Domain.Entities.Categories;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Categories;

public interface ICategoryRepository : IRepository<Category, int>, IApplicationService, IScopedService
{
    Task<Category> GetCategoryBrands(int id);
}
