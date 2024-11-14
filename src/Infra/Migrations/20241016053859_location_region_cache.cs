using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

/// <inheritdoc />
public partial class location_region_cache : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Locations_Regions_RegionId",
            table: "Locations");

        migrationBuilder.DropIndex(
            name: "IX_Locations_RegionId",
            table: "Locations");

        migrationBuilder.DropColumn(
            name: "RegionId",
            table: "Locations");

        migrationBuilder.AddColumn<string>(
            name: "RegionCache",
            table: "Locations",
            type: "TEXT",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RegionCache",
            table: "Locations");

        migrationBuilder.AddColumn<int>(
            name: "RegionId",
            table: "Locations",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Locations_RegionId",
            table: "Locations",
            column: "RegionId");

        migrationBuilder.AddForeignKey(
            name: "FK_Locations_Regions_RegionId",
            table: "Locations",
            column: "RegionId",
            principalTable: "Regions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
