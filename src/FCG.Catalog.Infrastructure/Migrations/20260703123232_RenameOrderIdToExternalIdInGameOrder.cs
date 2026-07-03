using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderIdToExternalIdInGameOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "GameOrders",
                newName: "ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrders_OrderId",
                table: "GameOrders",
                newName: "IX_GameOrders_ExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "GameOrders",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrders_ExternalId",
                table: "GameOrders",
                newName: "IX_GameOrders_OrderId");
        }
    }
}
