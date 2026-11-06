namespace FCG.Catalog.Infrastructure.DataAccess.Search;

internal class GameDocument
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
