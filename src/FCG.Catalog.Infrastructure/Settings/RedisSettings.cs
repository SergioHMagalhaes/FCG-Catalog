namespace FCG.Catalog.Infrastructure.Settings;

public class RedisSettings
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "fcg:catalog:";
    public int DefaultTtlMinutes { get; set; } = 15;
}
