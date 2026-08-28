using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Catalog.Infrastructure.DataAccess.Document.Documents;

public class ReviewDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid GameId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public List<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public int HelpfulVotes { get; set; }
}
