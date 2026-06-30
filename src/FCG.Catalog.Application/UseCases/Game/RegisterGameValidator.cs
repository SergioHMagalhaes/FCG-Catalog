using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.Game;

public class RegisterGameValidator : AbstractValidator<RequestGameJson>
{
    public RegisterGameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não deve exceder 100 caracteres.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(500).WithMessage("A descrição não deve exceder 500 caracteres.");
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório.")
            .GreaterThan(0).WithMessage("O ID da categoria deve ser maior que zero.");
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("O preço é obrigatório.");
    }
}
