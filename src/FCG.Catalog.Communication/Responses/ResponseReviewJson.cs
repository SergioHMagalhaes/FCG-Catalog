namespace FCG.Catalog.Communication.Responses;

public class ResponseReviewJson
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int HelpfulVotes { get; set; }
    public IReadOnlyCollection<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
