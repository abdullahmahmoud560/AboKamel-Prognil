using AboKamel.Application.Dtos.Dashboard.Offers;
using FluentValidation;

namespace AboKamel.Application.Validators.Dashboard.Offers;

public class OfferValidator : AbstractValidator<OfferRequestDto>
{
    public OfferValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DiscountPercentage)
            .NotNull().WithMessage("Discount percentage is required.")
            .InclusiveBetween(0.01m, 100m).WithMessage("Discount percentage must be between 0.01 and 100.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must(date => date > DateTime.Now)
            .WithMessage("Start date must be in the future.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than start date.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");
    }
}