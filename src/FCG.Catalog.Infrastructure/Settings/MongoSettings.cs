namespace FCG.Catalog.Infrastructure.Settings;

public class MongoSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string ReviewsCollectionName { get; set; } = default!;
}