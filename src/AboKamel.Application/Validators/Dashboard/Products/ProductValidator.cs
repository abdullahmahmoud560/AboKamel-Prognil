using Capsula.Application.Dtos.Dashboard.Products;
using FluentValidation;

namespace Capsula.Application.Validators.Dashboard.Products;

public class ProductValidator : AbstractValidator<ProductRequestDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(250).WithMessage("Product name must not exceed 250 characters.");

        RuleFor(x => x.MinimumQuantityPerOrder)
            .GreaterThan(0)
            .WithMessage("Minimum quantity per order must be greater than 0.");

        RuleFor(x => x.MaximumQuantityPerOrder)
            .GreaterThan(0)
            .WithMessage("Maximum quantity per order must be greater than 0.")
            .GreaterThanOrEqualTo(x => x.MinimumQuantityPerOrder)
            .WithMessage("Maximum quantity per order must be greater than or equal to minimum quantity per order.");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Brand is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        //RuleFor(x => x.AreaId)
        //    .NotEmpty().WithMessage("Area is required.");
    }
}
