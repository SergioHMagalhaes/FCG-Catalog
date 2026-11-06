using Elastic.Clients.Elasticsearch;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Domain.Shared.Pagination;
using FCG.Catalog.Infrastructure.DataAccess.Search;
using FCG.Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FCG.Catalog.Infrastructure.Services.Search;

public class GameSearchService : IGameSearchService
{
    private readonly ElasticsearchClient? _client;
    private readonly string _indexName;
    private readonly ILogger<GameSearchService> _logger;

    public GameSearchService(
        IOptions<ElasticsearchSettings> settings,
        ILogger<GameSearchService> logger,
        ElasticsearchClient? client = null)
    {
        _indexName = settings.Value.IndexName;
        _logger = logger;
        _client = client;
    }

    public async Task EnsureIndexCreatedAsync(CancellationToken cancellationToken = default)
    {
        if (_client == null)
            return;

        try
        {
            var existsResponse = await _client.Indices.ExistsAsync(_indexName, cancellationToken);
            if (existsResponse.Exists)
                return;

            await _client.Indices.CreateAsync<GameDocument>(_indexName, request => request
                .Mappings(m => m
                    .Properties(p => p
                        .Text(t => t.Name)
                        .Text(t => t.Description)
                        .Text(t => t.CategoryName)
                        .LongNumber(n => n.CategoryId)
                        .Keyword(k => k.ExternalId))),
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to ensure Elasticsearch index '{IndexName}' exists.", _indexName);
        }
    }

    public async Task IndexAsync(Game game, CancellationToken cancellationToken = default)
    {
        if (_client == null)
            return;

        try
        {
            var document = MapToDocument(game);

            await _client.IndexAsync(document, request => request
                .Index(_indexName)
                .Id(document.Id),
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to index game {GameId} in Elasticsearch.", game.Id);
        }
    }

    public async Task DeleteAsync(long gameId, CancellationToken cancellationToken = default)
    {
        if (_client == null)
            return;

        try
        {
            await _client.DeleteAsync<GameDocument>(gameId, request => request.Index(_indexName), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete game {GameId} from Elasticsearch.", gameId);
        }
    }

    public async Task<PagedResult<Game>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var empty = new PagedResult<Game>([], 0, page, pageSize);

        if (_client == null)
            return empty;

        try
        {
            var response = await _client.SearchAsync<GameDocument>(s => s
                .Indices(_indexName)
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(new[] { "name^3", "description", "categoryName" })
                        .Query(query)
                        .Fuzziness(new Fuzziness("AUTO")))),
                cancellationToken);

            if (!response.IsValidResponse)
            {
                _logger.LogWarning("Elasticsearch search request for query '{Query}' was not valid: {DebugInfo}", query, response.DebugInformation);
                return empty;
            }

            var items = response.Documents.Select(MapToDomain).ToList();
            var total = (int)response.Total;

            return new PagedResult<Game>(items, total, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to search games in Elasticsearch for query '{Query}'.", query);
            return empty;
        }
    }

    private static GameDocument MapToDocument(Game game)
    {
        return new GameDocument
        {
            Id = game.Id,
            ExternalId = game.ExternalId,
            Name = game.Name,
            Description = game.Description,
            Price = game.Price,
            CategoryId = game.CategoryId,
            CategoryName = game.Category?.Name ?? string.Empty
        };
    }

    private static Game MapToDomain(GameDocument document)
    {
        return new Game
        {
            Id = document.Id,
            ExternalId = document.ExternalId,
            Name = document.Name,
            Description = document.Description,
            Price = document.Price,
            CategoryId = document.CategoryId,
            Category = new Category
            {
                Id = document.CategoryId,
                Name = document.CategoryName
            }
        };
    }
}
