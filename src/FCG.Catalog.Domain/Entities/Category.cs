namespace FCG.Catalog.Domain.Entities;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Game> Games { get; set; } = [];
}
