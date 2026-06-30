using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;
using Moq;

namespace CommonTestUtilities.Repositories;

public class GameRepositoryBuilder
{
    private readonly Mock<IGameRepository> _repository;

    public GameRepositoryBuilder()
    {
        _repository = new Mock<IGameRepository>();
    }

    public GameRepositoryBuilder GetAll(PagedResult<Game> games, GamesFilter filter)
    {
        _repository.Setup(repository => repository.GetAll(
            It.Is<GamesFilter>(actualFilter =>
                actualFilter.Page == filter.Page
                && actualFilter.PageSize == filter.PageSize
                && actualFilter.OrderBy == filter.OrderBy
                && actualFilter.Desc == filter.Desc
                && actualFilter.Search == filter.Search))).ReturnsAsync(games);

        return this;
    }

    public void VerifyGetAll(GamesFilter filter)
    {
        _repository.Verify(repository => repository.GetAll(
            It.Is<GamesFilter>(actualFilter =>
                actualFilter.Page == filter.Page
                && actualFilter.PageSize == filter.PageSize
                && actualFilter.OrderBy == filter.OrderBy
                && actualFilter.Desc == filter.Desc
                && actualFilter.Search == filter.Search)), Times.Once);
    }

    public GameRepositoryBuilder GetById(Game game)
    {
        _repository.Setup(repository => repository.GetById(game.Id)).ReturnsAsync(game);

        return this;
    }

    public GameRepositoryBuilder GetByIdTracked(Game game)
    {
        _repository.Setup(repository => repository.GetByIdTracked(game.Id)).ReturnsAsync(game);

        return this;
    }

    public IGameRepository Build() => _repository.Object;
}
