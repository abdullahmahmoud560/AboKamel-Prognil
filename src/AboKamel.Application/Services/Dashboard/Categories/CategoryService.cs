using AutoMapper;
using Capsula.Application.Contracts.Dashboard.Categories;
using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Domain.Entities.Categories;
using Capsula.Domain.Repositories.Categories;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Services.Dashboard.Categories;

public class CategoryService : CrudService<CategoryRequestDto, Category, CategoryResponseDto, CategoryDetailedResponseDto, int>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Category> _logger;

    public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<Category> logger) : base(repository, mapper, logger)
    {
        _categoryRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<CategoryDetailedResponseDto>> GetCategoryBrandsAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryBrands(id);

        if (category is null)
        {
            return Result.Error("Could not find the category.");
        }

        return Result.Success(_mapper.Map<CategoryDetailedResponseDto>(category));
    }
}
