using Capsula.Application.Dtos.Mobile.Carts;
using FluentValidation;

namespace Capsula.Application.Validators.Mobile.Carts;

public class CartItemValidator : AbstractValidator<CartItemRequestDto>
{
    public CartItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.")
            .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100.");

        //RuleFor(x => x.CartId)
        //    .GreaterThan(0).WithMessage("CartId must be a valid reference.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a valid reference.");
    }
}
