namespace FCG.Catalog.Communication.Responses;

public class ResponseReviewsJson : ResponsePagedBase
{
    public List<ResponseReviewJson> Reviews { get; set; } = [];
}
