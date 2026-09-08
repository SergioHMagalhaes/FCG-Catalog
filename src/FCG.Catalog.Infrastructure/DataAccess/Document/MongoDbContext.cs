using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Infrastructure.DataAccess.Document.Documents;
using FCG.Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FCG.Catalog.Infrastructure.DataAccess.Document;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<ReviewDocument> Reviews =>
        _database.GetCollection<ReviewDocument>("reviews");
}
