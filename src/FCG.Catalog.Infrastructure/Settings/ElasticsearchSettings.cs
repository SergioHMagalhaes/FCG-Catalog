namespace FCG.Catalog.Infrastructure.Settings;

public class ElasticsearchSettings
{
    public const string SectionName = "Elasticsearch";

    public string Uri { get; set; } = string.Empty;
    public string IndexName { get; set; } = "games";
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ApiKey { get; set; }
}
