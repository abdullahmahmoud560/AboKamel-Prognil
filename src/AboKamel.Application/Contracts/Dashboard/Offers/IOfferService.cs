using AboKamel.Application.Dtos.Dashboard.Offers;
using AboKamel.Domain.Entities.Offers;
using Capsula.Application.Contracts;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Dashboard.Offers;

public interface IOfferService : ICrudService<OfferRequestDto, Offer, OfferResponseDto, OfferDetailedResponseDto, int>, IApplicationService, IScopedService
{
    public Task<ResultAbstract<List<OfferDetailedResponseDto>>> GetOffersWithDetailsAsync();
}