using Capsula.Application.Dtos.Authentication.Users.Customers;
using Capsula.Application.Dtos.Mobile.Prescriptions;

namespace Capsula.Application.Dtos.Mobile.Carts;

public class CartDetailedResponseDto : BaseResponseDto<int>
{
    public decimal TotalPrice { get; set; }

    public CustomerResponseDto Customer { get; set; }

    public ICollection<CartItemDetailedResponseDto> Items { get; set; } = new List<CartItemDetailedResponseDto>();

    public PrescriptionDetailedResponseDto Prescription { get; set; }
}
