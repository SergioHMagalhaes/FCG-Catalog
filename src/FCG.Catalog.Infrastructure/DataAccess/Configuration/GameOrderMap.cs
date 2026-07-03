using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Catalog.Infrastructure.DataAccess.Configuration;

public class GameOrderMap : IEntityTypeConfiguration<GameOrder>
{
    public void Configure(EntityTypeBuilder<GameOrder> builder)
    {
        builder.ToTable("GameOrders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id)
            .ValueGeneratedOnAdd();

        builder.Property(order => order.ExternalId)
            .IsRequired();

        builder.HasIndex(order => order.ExternalId)
            .IsUnique();

        builder.Property(order => order.GameId)
            .IsRequired();

        builder.HasOne(order => order.Game)
            .WithMany()
            .HasForeignKey(order => order.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(order => order.UserId)
            .IsRequired();

        builder.Property(order => order.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(order => order.Status)
            .IsRequired()
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<GameOrderStatus>(value))
            .HasMaxLength(30);

        builder.Property(order => order.CreatedOn)
            .IsRequired();

        builder.Property(order => order.ProcessedOn)
            .IsRequired(false);

        builder.HasIndex(order => order.UserId);

        builder.HasIndex(order => order.GameId);

        builder.HasIndex(order => order.Status);

        builder.HasIndex(order => new
        {
            order.UserId,
            order.GameId,
            order.Status
        });
    }
}
