namespace FCG.Catalog.Domain.Entities;

public class Review
{
    public Guid Id { get; private set; }
    public Guid GameId { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    public int HelpfulVotes { get; private set; }

    private readonly List<string> _tags = [];

    private Review() { }
    public Review(Guid gameId, Guid userId, string userName, int rating, string comment, List<string>? tags = null)
    {
        if (gameId == Guid.Empty)
            throw new ArgumentException("GameId não pode ser vazio.", nameof(gameId));

        if (userId == Guid.Empty)
            throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));

        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName não pode ser vazio.", nameof(userName));

        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating deve estar entre 1 e 5.", nameof(rating));

        Id = Guid.NewGuid();
        GameId = gameId;
        UserId = userId;
        UserName = userName;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
        HelpfulVotes = 0;

        if (tags != null)
        {
            foreach (var tag in tags)
                AddTag(tag);
        }
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return;

        if (_tags.Count >= 5)
            throw new InvalidOperationException("Não é possível adicionar mais de 5 tags.");

        if (tag.Length > 30)
            throw new ArgumentException("Cada tag deve ter no máximo 30 caracteres.");

        var normalized = tag.Trim().ToLowerInvariant();
        
        if (!_tags.Contains(normalized))
            _tags.Add(tag);
    }

    public void Update(Guid userId, int rating, string comment, IEnumerable<string>? tags = null)
    {
        if (userId != UserId)
            throw new UnauthorizedAccessException("Usuário não autorizado a atualizar esta review.");
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating deve estar entre 1 e 5.", nameof(rating));

        Rating = rating;
        Comment = comment;

        _tags.Clear();
        if (tags != null)
            foreach (var tag in tags)
                AddTag(tag);
    }

    public void MarkAsHelpful() => HelpfulVotes++;

    public static Review Rehydrate(
           Guid id,
           Guid gameId,
           Guid userId,
           string userName,
           int rating,
           string comment,
           IEnumerable<string> tags,
           DateTime createdAt,
           int helpfulVotes)
    {
        var review = new Review
        {
            Id = id,
            GameId = gameId,
            UserId = userId,
            UserName = userName,
            Rating = rating,
            Comment = comment,
            CreatedAt = createdAt,
            HelpfulVotes = helpfulVotes
        };

        review._tags.AddRange(tags);

        return review;
    }
}
