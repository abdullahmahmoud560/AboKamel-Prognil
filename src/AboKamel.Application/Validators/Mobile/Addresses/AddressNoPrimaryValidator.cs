using Capsula.Application.Dtos.Mobile.Addresses;
using FluentValidation;

public class AddressNoPrimaryValidator : AbstractValidator<AddressNotPrimaryRequestDto>
{
    public AddressNoPrimaryValidator()
    {
        RuleFor(x => x.Region)
            .MaximumLength(100).WithMessage("المنطقة يجب ألا تتجاوز 100 حرف");

        RuleFor(x => x.BuildingName)
            .MaximumLength(150).WithMessage("اسم المبنى يجب ألا يتجاوز 150 حرف");

        RuleFor(x => x.ApartmentNumber)
            .MaximumLength(20).WithMessage("رقم الشقة يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.FloorNumber)
            .MaximumLength(20).WithMessage("رقم الطابق يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(?:\+201|01)(0|1|2|5)[0-9]{8}$")
            .WithMessage("رقم الهاتف يجب أن يكون رقم موبايل مصري صحيح")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.DetailedAddress)
            .MaximumLength(500).WithMessage("العنوان التفصيلي يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrEmpty(x.DetailedAddress));

        RuleFor(x => x.DeliveryInstructions)
            .MaximumLength(250).WithMessage("تعليمات التوصيل يجب ألا تتجاوز 250 حرف");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("خط العرض يجب أن يكون بين -90 و 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("خط الطول يجب أن يكون بين -180 و 180")
            .When(x => x.Longitude.HasValue);
    }
}