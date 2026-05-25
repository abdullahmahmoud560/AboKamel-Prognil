using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using FluentValidation;

namespace AboKamel.Application.Validators.Dashboard.SellingUnits;

public class ProductSellingUnitValidator : AbstractValidator<ProductSellingUnitRequestDto>
{
    public ProductSellingUnitValidator()
    {
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
    }
}
