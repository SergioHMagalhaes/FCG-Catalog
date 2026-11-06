using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.Games.Search;

public class SearchGamesValidator : AbstractValidator<RequestSearchGamesJson>
{
    public SearchGamesValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty().WithMessage("O termo de busca é obrigatório.");
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}
