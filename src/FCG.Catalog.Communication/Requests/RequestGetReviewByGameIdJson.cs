using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Requests;

public class RequestGetReviewByGameIdJson : RequestPagedBase
{
    public ReviewOrderBy OrderBy { get; set; } = ReviewOrderBy.CreatedAt;
    public Guid GameId { get; set; }
}
