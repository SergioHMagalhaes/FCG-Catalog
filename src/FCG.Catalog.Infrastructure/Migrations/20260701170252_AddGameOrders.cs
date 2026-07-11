using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FCG.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGameOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameOrders_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameOrders_GameId",
                table: "GameOrders",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameOrders_OrderId",
                table: "GameOrders",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameOrders_Status",
                table: "GameOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GameOrders_UserId",
                table: "GameOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameOrders_UserId_GameId_Status",
                table: "GameOrders",
                columns: new[] { "UserId", "GameId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameOrders");
        }
    }
}
