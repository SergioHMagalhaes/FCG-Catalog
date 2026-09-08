namespace FCG.Catalog.Communication.Requests;

public class RequestReviewUpdateJson
{
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public List<string> Tags { get; set; } = [];
}
