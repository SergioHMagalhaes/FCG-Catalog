using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.GameOrders;

public class PlaceGameOrderValidator : AbstractValidator<RequestPlaceGameOrderJson>
{
    public PlaceGameOrderValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("O ID do jogo é obrigatório.")
            .ChildRules(gameId =>
            {
                gameId.RuleFor(x => x)
                    .Must(id => Guid.TryParse(id.ToString(), out _))
                    .WithMessage("O ID do jogo deve ser um GUID válido.");
            });
    }
}
