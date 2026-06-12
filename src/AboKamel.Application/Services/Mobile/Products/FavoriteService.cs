using AutoMapper;
using Capsula.Application.Contracts.Mobile.Products;
using Capsula.Application.Dtos.Mobile.Products;
using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Application.Services.Mobile.Products;

public class FavoriteService : CrudService<FavoriteRequestDto, Favorite, FavoriteResponseDto, FavoriteDetailedResponseDto, int>, IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Favorite> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FavoriteService(IFavoriteRepository repository, IMapper mapper, ILogger<Favorite> logger, IHttpContextAccessor httpContextAccessor)
        : base(repository, mapper, logger)
    {
        _favoriteRepository = repository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }
        return userId;
    }

    public async Task<ResultAbstract<IEnumerable<FavoriteResponseDto>>> GetAllFavoritesByUserIdAsync(string? userId = null)
    {
        var currentUserId = userId ?? GetCurrentUserId();
        var favorites = await _favoriteRepository.GetFavoritesByUserIdAsync(currentUserId);
        return Result.Success(_mapper.Map<IEnumerable<FavoriteResponseDto>>(favorites)); 
    }

    public async Task<ResultAbstract<bool>> DeleteFavoriteByProductIdAsync(FavoriteRequestDto favoriteRequest, string? userId = null)
    {
        var currentUserId = userId ?? GetCurrentUserId();
        var favorite = await _favoriteRepository.GetFavoriteByProductIdAsync(currentUserId, favoriteRequest.ProductId);

        if(favorite is null)
        {
            string message = $"Could not find favorite product with product id {favoriteRequest.ProductId} and user id {currentUserId}";
            _logger.LogWarning(message);
            return Result.Error(message);
        }

        var isFavoriteDeleted = await _favoriteRepository.DeleteAsync(favorite);

        if(!isFavoriteDeleted)
        {
            string message = $"Could not delete favorite product with product id {favoriteRequest.ProductId} and user id {currentUserId}";
            _logger.LogWarning(message);
            return Result.Error(message);
        }

        return true;
    }
}
