using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using FluentValidation;

namespace AboKamel.Application.Validators.Dashboard.SellingUnits;

public class SellingUnitValidator : AbstractValidator<SellingUnitRequestDto>
{
    public SellingUnitValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
