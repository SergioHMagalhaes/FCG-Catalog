using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Categories.GetAll;

public interface IGetAllCategoryUseCase
{
    Task<ResponseCategoriesJson> Execute();
}
