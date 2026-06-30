namespace FCG.Catalog.Communication.Responses;

public class ResponseGameJson
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ResponseCategoryJson Category { get; set; } = default!;
}
