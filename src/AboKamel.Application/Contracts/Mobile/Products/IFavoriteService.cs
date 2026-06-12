using Capsula.Application.Dtos.Mobile.Products;
using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Products;

public interface IFavoriteService : ICrudService<FavoriteRequestDto, Favorite, FavoriteResponseDto, FavoriteDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<IEnumerable<FavoriteResponseDto>>> GetAllFavoritesByUserIdAsync(string? userId = null);
    Task<ResultAbstract<bool>> DeleteFavoriteByProductIdAsync(FavoriteRequestDto favoriteRequest, string? userId = null);
}