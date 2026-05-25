using Capsula.Application.Dtos.Authentication.Users.Customers;
using FluentValidation;

namespace Capsula.Application.Validators.Mobile.Customers;

public class CustomerValidator : AbstractValidator<CustomerRequestDto>
{
    public CustomerValidator()
    {
        RuleFor(x => x.UserName)
             .NotEmpty().WithMessage("Username is required.")
             .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
             .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.CustomPassword)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name must not exceed 200 characters.");
    }
}
