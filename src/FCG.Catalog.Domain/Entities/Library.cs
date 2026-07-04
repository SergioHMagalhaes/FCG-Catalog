namespace FCG.Catalog.Domain.Entities;

public class Library
{
    public long Id { get; private set; }
    public Guid ExternalId { get; private set; }
    public Guid UserId { get; private set; }
    private readonly List<Game> _games = [];
    public IReadOnlyCollection<Game> Games => _games.AsReadOnly();
}