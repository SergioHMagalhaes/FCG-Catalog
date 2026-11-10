using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Domain.Shared.Pagination;
using Moq;

namespace CommonTestUtilities.Services;

public class GameSearchServiceBuilder
{
    private readonly Mock<IGameSearchService> _service;

    public GameSearchServiceBuilder()
    {
        _service = new Mock<IGameSearchService>();
    }

    public GameSearchServiceBuilder Search(PagedResult<Game> result)
    {
        _service
            .Setup(service => service.SearchAsync(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return this;
    }

    public IGameSearchService Build() => _service.Object;

    public void VerifyIndexOnce(Func<Game, bool>? match = null)
    {
        match ??= _ => true;

        _service.Verify(service => service.IndexAsync(
            It.Is<Game>(game => match(game)), It.IsAny<CancellationToken>()), Times.Once);
    }

    public void VerifyIndexNever()
    {
        _service.Verify(service => service.IndexAsync(
            It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    public void VerifyDeleteOnce(long gameId)
    {
        _service.Verify(service => service.DeleteAsync(gameId, It.IsAny<CancellationToken>()), Times.Once);
    }

    public void VerifyDeleteNever()
    {
        _service.Verify(service => service.DeleteAsync(
            It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
