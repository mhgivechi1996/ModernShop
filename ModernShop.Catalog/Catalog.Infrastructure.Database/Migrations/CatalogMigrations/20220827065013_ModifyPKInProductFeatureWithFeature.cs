using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Database.Migrations.CatalogMigrations
{
    public partial class ModifyPKInProductFeatureWithFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatureValues",
                schema: "Catalog",
                table: "ProductFeatureValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatureValues_ProductId",
                schema: "Catalog",
                table: "ProductFeatureValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatureValues",
                schema: "Catalog",
                table: "ProductFeatureValues",
                columns: new[] { "ProductId", "FeatureId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatureValues",
                schema: "Catalog",
                table: "ProductFeatureValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatureValues",
                schema: "Catalog",
                table: "ProductFeatureValues",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureValues_ProductId",
                schema: "Catalog",
                table: "ProductFeatureValues",
                column: "ProductId");
        }
    }
}
