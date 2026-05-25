using AboKamel.Application.Dtos.Dashboard.Debts;
using FluentValidation;

namespace AboKamel.Application.Validators.Dashboard.Debts;

public class DebtValidator : AbstractValidator<DebtRequestDto>
{
    public DebtValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required.");

        RuleFor(x => x.DebitCredit)
            .IsInEnum().WithMessage("Invalid Debit/Credit value.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}
