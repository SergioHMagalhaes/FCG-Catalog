namespace FCG.Catalog.Communication.Requests;

public abstract class RequestPagedBase
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public bool Desc { get; set; } = false;
}