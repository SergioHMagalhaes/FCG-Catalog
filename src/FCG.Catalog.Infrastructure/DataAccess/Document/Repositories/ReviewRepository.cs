using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Enums;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;
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

    public async Task<PagedResult<Review>> GetByGameId(ReviewFilter filter)
    {
        var filterBuilder = Builders<ReviewDocument>.Filter.Eq(x => x.GameId, filter.GameId);

        var sortBuilder = Builders<ReviewDocument>.Sort;
        var sort = (filter.OrderBy, filter.Desc) switch
        {
            (ReviewOrderBy.Rating, true) => sortBuilder.Descending(x => x.Rating),
            (ReviewOrderBy.Rating, false) => sortBuilder.Ascending(x => x.Rating),
            (ReviewOrderBy.HelpfulVotes, true) => sortBuilder.Descending(x => x.HelpfulVotes),
            (ReviewOrderBy.HelpfulVotes, false) => sortBuilder.Ascending(x => x.HelpfulVotes),
            (ReviewOrderBy.CreatedAt, true) => sortBuilder.Descending(x => x.CreatedAt),
            _ => sortBuilder.Ascending(x => x.CreatedAt),
        };

        var total = await _collection.CountDocumentsAsync(filterBuilder);

        var documents = await _collection
            .Find(filterBuilder)
            .Sort(sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();
        
        var items = documents.Select(ReviewMapper.ToDomain).ToList();

        return new PagedResult<Review>(items, (int)total, filter.Page, filter.PageSize);
    }
}
