using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Payments;
using Capsula.Core.Enums;

namespace Capsula.Application.Services.Mobile.Payments;

public static class PaymobMappingExtension
{
    public static PaymentIntentionRequestDto ToPaymobIntention(
        this CartDetailedResponseDto cart,
        string paymentMethod,
        string currency = "EGP"
        )
    {
        return new PaymentIntentionRequestDto
        {
            Amount = (int)(cart.TotalPrice * 100),
            Currency = currency,
            PaymentMethods = new List<string> { paymentMethod },

            Items = cart.Items.Select(i => new PaymobItemDto
            {
                Name = i.Product.Name,
                Amount = (int)i.Product.Price * 100,
                //Amount = (int)(
                //    (i.Unit == ProductUnitType.Box
                //        ? i.Product.Price
                //        : i.Product.StripPrice
                //    ) * 100),
                Description = "N/A",
                Quantity = i.Quantity
            }).ToList(),

            BillingData = new PaymobBillingDataDto
            {
                FirstName = cart.Customer?.FullName?.Split(' ').FirstOrDefault() ?? "N/A",
                LastName = cart.Customer?.FullName?.Split(' ').Skip(1).FirstOrDefault() ?? "N/A",
                Email = cart.Customer?.Email ?? "N/A",
                PhoneNumber = cart.Customer?.PhoneNumber ?? "N/A",
                Country = "EG"
            },
            SpecialReference = Guid.NewGuid().ToString()
        };
    }
}
