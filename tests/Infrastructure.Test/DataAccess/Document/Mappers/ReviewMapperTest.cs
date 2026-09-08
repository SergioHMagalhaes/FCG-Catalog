using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Infrastructure.DataAccess.Document.Documents;
using FCG.Catalog.Infrastructure.DataAccess.Document.Mappers;

namespace Infrastructure.Test.DataAccess.Document.Mappers;

public class ReviewMapperTest
{
    [Fact]
    public void ToDocument_Should_Map_All_Fields()
    {
        var review = new Review(Guid.NewGuid(), Guid.NewGuid(), "player_one", 5, "Great game", ["fun", "co-op"]);

        var document = ReviewMapper.ToDocument(review);

        Assert.Equal(review.Id, document.Id);
        Assert.Equal(review.GameId, document.GameId);
        Assert.Equal(review.UserId, document.UserId);
        Assert.Equal(review.UserName, document.UserName);
        Assert.Equal(review.Rating, document.Rating);
        Assert.Equal(review.Comment, document.Comment);
        Assert.Equal(review.Tags, document.Tags);
        Assert.Equal(review.CreatedAt, document.CreatedAt);
        Assert.Equal(review.HelpfulVotes, document.HelpfulVotes);
    }

    [Fact]
    public void ToDocument_Should_Map_Empty_Tags()
    {
        var review = new Review(Guid.NewGuid(), Guid.NewGuid(), "player_one", 3, "Ok game");

        var document = ReviewMapper.ToDocument(review);

        Assert.Empty(document.Tags);
    }

    [Fact]
    public void ToDocument_Should_Return_A_New_List_Instance_For_Tags()
    {
        var review = new Review(Guid.NewGuid(), Guid.NewGuid(), "player_one", 4, "Nice", ["tag"]);

        var document = ReviewMapper.ToDocument(review);

        Assert.NotSame(review.Tags, document.Tags);
    }

    [Fact]
    public void ToDomain_Should_Map_All_Fields()
    {
        var document = new ReviewDocument
        {
            Id = Guid.NewGuid(),
            GameId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserName = "player_two",
            Rating = 4,
            Comment = "Pretty good",
            Tags = ["story", "graphics"],
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            HelpfulVotes = 7
        };

        var review = ReviewMapper.ToDomain(document);

        Assert.Equal(document.Id, review.Id);
        Assert.Equal(document.GameId, review.GameId);
        Assert.Equal(document.UserId, review.UserId);
        Assert.Equal(document.UserName, review.UserName);
        Assert.Equal(document.Rating, review.Rating);
        Assert.Equal(document.Comment, review.Comment);
        Assert.Equal(document.Tags, review.Tags);
        Assert.Equal(document.CreatedAt, review.CreatedAt);
        Assert.Equal(document.HelpfulVotes, review.HelpfulVotes);
    }

    [Fact]
    public void ToDomain_Should_Map_Empty_Tags()
    {
        var document = new ReviewDocument
        {
            Id = Guid.NewGuid(),
            GameId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserName = "player_two",
            Rating = 2,
            Comment = "Meh",
            Tags = [],
            CreatedAt = DateTime.UtcNow,
            HelpfulVotes = 0
        };

        var review = ReviewMapper.ToDomain(document);

        Assert.Empty(review.Tags);
    }

    [Fact]
    public void RoundTrip_ToDocument_Then_ToDomain_Should_Preserve_Data()
    {
        var original = new Review(Guid.NewGuid(), Guid.NewGuid(), "player_three", 5, "Excellent", ["replayable"]);
        original.MarkAsHelpful();

        var document = ReviewMapper.ToDocument(original);
        var roundTripped = ReviewMapper.ToDomain(document);

        Assert.Equal(original.Id, roundTripped.Id);
        Assert.Equal(original.GameId, roundTripped.GameId);
        Assert.Equal(original.UserId, roundTripped.UserId);
        Assert.Equal(original.UserName, roundTripped.UserName);
        Assert.Equal(original.Rating, roundTripped.Rating);
        Assert.Equal(original.Comment, roundTripped.Comment);
        Assert.Equal(original.Tags, roundTripped.Tags);
        Assert.Equal(original.CreatedAt, roundTripped.CreatedAt);
        Assert.Equal(original.HelpfulVotes, roundTripped.HelpfulVotes);
    }
}
