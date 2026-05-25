using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Contracts.Voices;
using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Core.Enums;
using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Entities.Prescriptions;
using Capsula.Domain.Repositories.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IImageService _imageService;
    private readonly IVoiceService _voiceService;
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Cart> _logger;

    public CartService(ICartRepository cartRepository, IMapper mapper, ILogger<Cart> logger, IImageService imageService, IVoiceService voiceService, IPrescriptionRepository prescriptionRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
        _imageService = imageService;
        _voiceService = voiceService;
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<bool> CartExistsAsync(int cartId)
    {
        return await _cartRepository.ExistsAsync(cartId);
    }

    public async Task<Cart> GetCustomerCartAsync(string customerId)
    {
        return await _cartRepository.GetCustomerCartAsync(customerId);
    }

    public async Task<ResultAbstract<CartDetailedResponseDto>> GetCustomerCartDetailsAsync(string customerId)
    {
        var cart = await _cartRepository.GetCustomerCartDetailsAsync(customerId);

        var cartDetails = _mapper.Map<CartDetailedResponseDto>(cart);

        cartDetails.TotalPrice = cart.Items.Sum(i => i.ProductSellingUnit.Price * i.Quantity);
        //cartDetails.TotalPrice = cart.Items.Sum(i =>
        //    i.Unit == ProductUnitType.Box
        //        ? i.Product.Price * i.Quantity
        //        : i.Product.StripPrice * i.Quantity);


        if (cartDetails.Prescription is not null)
        {
            foreach (var voice in cartDetails.Prescription.VoiceRecords)
            {
                voice.VoiceUrl = _voiceService.GenerateUrl(voice.VoiceUrl);
            }

            foreach (var image in cartDetails.Prescription.PrescriptionImages)
            {
                image.ImageUrl = _imageService.GenerateUrl(image.ImageUrl);
            }
        }

        return Result.Success(cartDetails);
    }

    public async Task<Cart> InitializeCustomerCartAsync(string customerId)
    {
        var cart = new Cart
        {
            CustomerId = customerId
        };

        var isCartAdded = await _cartRepository.AddAsync(cart);

        if (!isCartAdded) 
        {
            _logger.LogError($"Could not initialize cart for customer: {customerId}");
            return null;
        }

        var dbContext = _cartRepository.GetDbContext();

        var prescription = new Prescription
        {
            CartId = cart.Id
        };

        cart.Prescription = prescription;

        dbContext.SaveChanges();

        return cart;
    }

    public async Task<ResultAbstract<bool>> AddPrescriptionImageToCartAsync(string customerId, IFormFile ImageFile)
    {
        _imageService.ValidateImage(ImageFile);

        var relativePath = _imageService.GetImageRelativePath(ImageFile);
        await _imageService.SaveImageAsync(relativePath, ImageFile);

        var cart = await _cartRepository.GetCustomerCartAsync(customerId);

        if (cart is null)
        {
            cart = await InitializeCustomerCartAsync(customerId);
        }

        var prescription = await _prescriptionRepository.GetCartPrescription(cart.Id);

        if (prescription is null)
        {
            prescription = new Prescription
            {
                CartId = cart.Id
            };
            await _prescriptionRepository.AddAsync(prescription);
        }

        var prescriptionImage = new PrescriptionImage
        {
            PrescriptionId = prescription.Id,
            ImagePath = relativePath
        };

        prescription.PrescriptionImages.Add(prescriptionImage);
        await _prescriptionRepository.EditAsync(prescription);

        return Result.Success(true);
    }

    public async Task<ResultAbstract<bool>> AddPrescriptionVoiceRecordToCartAsync(string customerId, IFormFile VoiceFile)
    {
        _voiceService.ValidateVoice(VoiceFile);

        var relativePath = _voiceService.GetVoiceRelativePath(VoiceFile);
        await _voiceService.SaveVoiceAsync(relativePath, VoiceFile);

        var cart = await _cartRepository.GetCustomerCartAsync(customerId);

        if (cart is null)
        {
            cart = await InitializeCustomerCartAsync(customerId);
        }

        var prescription = await _prescriptionRepository.GetCartPrescription(cart.Id);

        if (prescription is null)
        {
            prescription = new Prescription
            {
                CartId = cart.Id
            };
            await _prescriptionRepository.AddAsync(prescription);
        }

        var vooiceRecord = new VoiceRecord
        {
            PrescriptionId = prescription.Id,
            VoicePath = relativePath
        };

        prescription.VoiceRecords.Add(vooiceRecord);
        await _prescriptionRepository.EditAsync(prescription);

        return true;
    }
}
