using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Categories.GetById;

public interface IGetCategoryByIdUseCase
{
    Task<ResponseCategoryJson> Execute(long id);
}
