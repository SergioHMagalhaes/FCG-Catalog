namespace FCG.Catalog.Application.UseCases.Category.Delete;

public interface IDeleteCategoryUseCase
{
    Task Execute(long id);
}
