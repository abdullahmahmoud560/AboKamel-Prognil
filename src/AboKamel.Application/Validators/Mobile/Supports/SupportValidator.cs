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

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^(?:\+201|01)(0|1|2|5)[0-9]{8}$")
            .WithMessage("Phone number must be a valid Egyptian mobile number (e.g., +2010XXXXXXXX or 010XXXXXXXX)");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");

        RuleFor(s => s.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
