namespace FCG.Catalog.Application.UseCases.Game.Delete;

public interface IDeleteGameUseCase
{
    Task Execute(long id);
}
