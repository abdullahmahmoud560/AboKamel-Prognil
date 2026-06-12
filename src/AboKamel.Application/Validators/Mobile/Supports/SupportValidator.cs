using Capsula.Application.Dtos.Mobile.Supports;
using FluentValidation;

namespace Capsula.Application.Validators.Mobile.Supports;

public class SupportValidator : AbstractValidator<SupportRequestDto>
{
    public SupportValidator()
    {
        RuleFor(s => s.FullName)
            .NotEmpty().WithMessage("Full Name is required.")
            .MaximumLength(150).WithMessage("Full Name cannot exceed 150 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters long.")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.");
    }
}