using FCG.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Catalog.Infrastructure.DataAccess.Configuration;

internal class GameMap : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(game => game.Id);

        builder.Property(game => game.Id)
            .ValueGeneratedOnAdd();

        builder.Property(game => game.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(game => game.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(game => game.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(game => game.CategoryId)
            .IsRequired();

        builder.HasOne(game => game.Category)
            .WithMany(category => category.Games)
            .HasForeignKey(game => game.CategoryId);
    }
}