using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class IdsinCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Locations_LocationId",
                table: "Elements");

            migrationBuilder.DropIndex(
                name: "IX_Elements_LocationId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Elements");

            migrationBuilder.RenameColumn(
                name: "OwnerNamesCache",
                table: "Substations",
                newName: "RegionCache");

            migrationBuilder.AddColumn<string>(
                name: "OwnerIdsCache",
                table: "Substations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegionCache",
                table: "Elements",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerIdsCache",
                table: "Substations");

            migrationBuilder.DropColumn(
                name: "RegionCache",
                table: "Elements");

            migrationBuilder.RenameColumn(
                name: "RegionCache",
                table: "Substations",
                newName: "OwnerNamesCache");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Elements",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Elements_LocationId",
                table: "Elements",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Locations_LocationId",
                table: "Elements",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
