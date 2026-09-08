using FCG.Catalog.Communication.Requests;
using FluentValidation;

namespace FCG.Catalog.Application.UseCases.Reviews.Update;

public class UpdateReviewValidator : AbstractValidator<RequestReviewUpdateJson>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Rating)
            .NotEmpty().WithMessage("A avaliação é obrigatória.")
            .InclusiveBetween(1, 5).WithMessage("A avaliação deve estar entre 1 e 5.");
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("O comentário é obrigatório.")
            .MaximumLength(500).WithMessage("O comentário não deve exceder 500 caracteres.");

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.Count <= 5)
                .WithMessage("Uma review pode ter no máximo 5 tags.")
            .Must(tags => tags == null || tags.All(t => t.Length <= 30))
                .WithMessage("Cada tag deve ter no máximo 30 caracteres.")
            .Must(tags => tags == null || tags.Distinct(StringComparer.OrdinalIgnoreCase).Count() == tags.Count)
                .WithMessage("Não é permitido repetir tags.");
    }
}
