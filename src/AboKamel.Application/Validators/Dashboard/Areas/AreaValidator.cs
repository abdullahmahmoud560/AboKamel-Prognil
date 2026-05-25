using AboKamel.Application.Dtos.Dashboard.Areas;
using FluentValidation;

namespace AboKamel.Application.Validators.Dashboard.Areas;

public class AreaValidator : AbstractValidator<AreaRequestDto>
{
    public AreaValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}