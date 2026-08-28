using FCG.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Catalog.Infrastructure.DataAccess.Relational.Configuration;

internal class LibraryMap : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.ToTable("Libraries");

        builder.HasKey(library => library.Id);

        builder.Property(library => library.Id)
            .ValueGeneratedOnAdd();

        builder.Property(library => library.ExternalId)
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(library => library.ExternalId)
            .IsUnique();

        builder.Property(library => library.UserId)
            .IsRequired();

        builder.HasIndex(library => library.UserId)
            .IsUnique();

        builder
            .HasMany(library => library.Games)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "LibraryGames",
                right => right
                    .HasOne<Game>()
                    .WithMany()
                    .HasForeignKey("GameId")
                    .OnDelete(DeleteBehavior.Restrict),
                left => left
                    .HasOne<Library>()
                    .WithMany()
                    .HasForeignKey("LibraryId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("LibraryGames");

                    join.HasKey("LibraryId", "GameId");

                    join.Property<long>("LibraryId")
                        .IsRequired();

                    join.Property<long>("GameId")
                        .IsRequired();
                }
            );
        }
}