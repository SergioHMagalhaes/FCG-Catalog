using FCG.Catalog.Communication.Requests;

namespace FCG.Catalog.Application.UseCases.Categories.Update;

public interface IUpdateCategoryUseCase
{
    Task Execute(long id, RequestCategoryJson request);
}
