using FCG.Catalog.Communication.Requests;

namespace FCG.Catalog.Application.UseCases.Category.Update;

public interface IUpdateCategoryUseCase
{
    Task Execute(long id, RequestCategoryJson request);
}
