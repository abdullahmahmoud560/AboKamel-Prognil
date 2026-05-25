using AboKamel.Application.Contracts.Dashboard.Offers;
using AboKamel.Application.Dtos.Dashboard.Offers;
using AboKamel.Domain.Entities.Offers;
using AboKamel.Domain.Repositories.Offers;
using AutoMapper;
using Capsula.Application.Services;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace AboKamel.Application.Services.Dashboard.Offers;

public class OfferService : CrudService<OfferRequestDto, Offer, OfferResponseDto, OfferDetailedResponseDto, int>, IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Offer> _logger;

    public OfferService(IOfferRepository repository, IMapper mapper, ILogger<Offer> logger) : base(repository, mapper, logger)
    {
        _offerRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<List<OfferDetailedResponseDto>>> GetOffersWithDetailsAsync()
    {
        var offers = await _offerRepository.GetOfferWithDetailsAsync();
        return Result.Success(_mapper.Map<List<OfferDetailedResponseDto>>(offers));
    }
}