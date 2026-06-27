using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Category.GetAll;

public interface IGetAllCategoryUseCase
{
    Task<ResponseCategoriesJson> Execute();
}
