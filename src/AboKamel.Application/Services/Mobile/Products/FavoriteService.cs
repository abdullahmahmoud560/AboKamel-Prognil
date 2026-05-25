using AutoMapper;
using Capsula.Application.Contracts.Mobile.Products;
using Capsula.Application.Dtos.Mobile.Products;
using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Products;

public class FavoriteService : CrudService<FavoriteRequestDto, Favorite, FavoriteResponseDto, FavoriteDetailedResponseDto, int>, IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Favorite> _logger;

    public FavoriteService(IFavoriteRepository repository, IMapper mapper, ILogger<Favorite> logger)
        : base(repository, mapper, logger)
    {
        _favoriteRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<IEnumerable<FavoriteResponseDto>>> GetAllFavoritesByUserIdAsync(string userId)
    {
        var favorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
        return Result.Success(_mapper.Map<IEnumerable<FavoriteResponseDto>>(favorites)); 
    }

    public async Task<ResultAbstract<bool>> DeleteFavoriteByProductIdAsync(FavoriteRequestDto favoriteRequest)
    {
        var favorite = await _favoriteRepository.GetFavoriteByProductIdAsync(favoriteRequest.UserId, favoriteRequest.ProductId);

        if(favorite is null)
        {
            string message = $"Could not find favorite product with product id {favoriteRequest.ProductId} and user id {favoriteRequest.UserId}";
            _logger.LogWarning(message);
            return Result.Error(message);
        }

        var isFavoriteDeleted = await _favoriteRepository.DeleteAsync(favorite);

        if(!isFavoriteDeleted)
        {
            string message = $"Could not delete favorite product with product id {favoriteRequest.ProductId} and user id {favoriteRequest.UserId}";
            _logger.LogWarning(message);
            return Result.Error(message);
        }

        return true;
    }
}
