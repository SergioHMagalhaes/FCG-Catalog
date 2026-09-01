namespace FCG.Catalog.Communication.Requests;

public class RequestReviewJson
{
    public Guid GameId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public List<string> Tags { get; set; } = [];
}
