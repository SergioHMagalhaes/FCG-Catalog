using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.Category;

public class RegisterCategoryValidator : AbstractValidator<RequestCategoryJson>
{
    public RegisterCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não deve exceder 100 caracteres.");
    }
}
