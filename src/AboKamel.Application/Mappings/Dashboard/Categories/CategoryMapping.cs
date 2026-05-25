using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Application.Mappings.Resolvers.Categories;
using Capsula.Application.Mappings.Resolvers.Categoriesp;
using Capsula.Domain.Entities.Categories;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Dashboard.Categories;

public static class CategoryMapping
{
    public static void AddCategoryMapping(this MappingProfiles map)
    {
        map.CreateMap<CategoryRequestDto, Category>().ForMember(dest => dest.ImagePath, src => src.MapFrom<CategoryImageSaveResolver>());

        map.CreateMap<Category, CategoryResponseDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom<CategoryImageGenerateResolver>());

        map.CreateMap<Category, CategoryDetailedResponseDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom<CategoryDetailsImageGenerateResolver>());
    }
}
