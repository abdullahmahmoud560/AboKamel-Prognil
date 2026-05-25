using Capsula.Application.Dtos.Mobile.Addresses;
using FluentValidation;

namespace Capsula.Application.Validators.Mobile.Addresses;

public class AddressNoPrimaryValidator : AbstractValidator<AddressNotPrimaryRequestDto>
{
    public AddressNoPrimaryValidator()
    {
        RuleFor(x => x.Region)
            .MaximumLength(100).WithMessage("Region must not exceed 100 characters");

        RuleFor(x => x.BuildingName)
            .MaximumLength(150).WithMessage("Building name must not exceed 150 characters");

        RuleFor(x => x.ApartmentNumber)
            .MaximumLength(20).WithMessage("Apartment number must not exceed 20 characters");

        RuleFor(x => x.FloorNumber)
            .MaximumLength(20).WithMessage("Floor number must not exceed 20 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^(?:\+201|01)(0|1|2|5)[0-9]{8}$")
            .WithMessage("Phone number must be a valid Egyptian mobile number (e.g., +2010XXXXXXXX or 010XXXXXXXX)");

        RuleFor(x => x.DetailedAddress)
            .NotEmpty().WithMessage("Detailed address is required")
            .MaximumLength(20).WithMessage("Detailed Address must not exceed 500 characters");

        RuleFor(x => x.DeliveryInstructions)
            .MaximumLength(250).WithMessage("Delivery instructions must not exceed 250 characters");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
    }
}
