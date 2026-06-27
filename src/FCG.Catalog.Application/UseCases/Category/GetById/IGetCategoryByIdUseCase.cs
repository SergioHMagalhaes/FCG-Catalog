using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Category.GetById;

public interface IGetCategoryByIdUseCase
{
    Task<ResponseCategoryJson> Execute(long id);
}
