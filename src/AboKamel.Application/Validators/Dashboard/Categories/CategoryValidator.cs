using Capsula.Application.Dtos.Dashboard.Categories;
using FluentValidation;

namespace Capsula.Application.Validators.Dashboard.Categories;

public class CategoryValidator : AbstractValidator<CategoryRequestDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Slug)
            .MaximumLength(200);
    }
}
