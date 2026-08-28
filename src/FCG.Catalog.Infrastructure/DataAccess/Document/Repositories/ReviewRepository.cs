using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Infrastructure.DataAccess.Document.Documents;
using FCG.Catalog.Infrastructure.DataAccess.Document.Mappers;
using MongoDB.Driver;

namespace FCG.Catalog.Infrastructure.DataAccess.Document.Repositories;

internal class ReviewRepository : IReviewRepository
{
    private readonly IMongoCollection<ReviewDocument> _collection;

    public ReviewRepository(MongoDbContext context)
    {
        _collection = context.Reviews;
    }
    public async Task Add(Review review)
    {
        await _collection.InsertOneAsync(ReviewMapper.ToDocument(review));
    }
}
