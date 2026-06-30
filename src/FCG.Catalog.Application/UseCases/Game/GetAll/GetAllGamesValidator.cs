using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.Game.GetAll;

public class GetAllGamesValidator : AbstractValidator<RequestGetAllGamesJson>
{
    public GetAllGamesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        RuleFor(x => x.OrderBy)
                .IsInEnum()
                .WithMessage("OrderBy inválido.");
    }
}
