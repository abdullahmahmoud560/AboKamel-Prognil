using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Prescriptions;
using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Entities.Prescriptions;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Mobile.Carts;

public static class CartMapping
{
    public static void AddCartMapping(this MappingProfiles map)
    {
        map.CreateMap<CartRequestDto, Cart>();
        map.CreateMap<Cart, CartResponseDto>();
        map.CreateMap<Cart, CartDetailedResponseDto>();
        map.CreateMap<Prescription, PrescriptionDetailedResponseDto>();
        map.CreateMap<PrescriptionImage, PrescriptionImageResponserDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom(src => src.ImagePath));
        map.CreateMap<VoiceRecord, VoiceRecordResponseDto>().ForMember(dest => dest.VoiceUrl, src => src.MapFrom(src => src.VoicePath));
    }
}