namespace FCG.Catalog.Application.UseCases.Games.Delete;

public interface IDeleteGameUseCase
{
    Task Execute(long id);
}
