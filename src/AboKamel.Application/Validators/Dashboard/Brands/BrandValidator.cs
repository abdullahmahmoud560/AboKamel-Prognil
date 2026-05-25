using Capsula.Application.Dtos.Dashboard.Brands;
using FluentValidation;

namespace Capsula.Application.Validators.Dashboard.Brands;

public class BrandValidator : AbstractValidator<BrandRequestDto>
{
    public BrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Slug)
            .MaximumLength(200);
    }
}
