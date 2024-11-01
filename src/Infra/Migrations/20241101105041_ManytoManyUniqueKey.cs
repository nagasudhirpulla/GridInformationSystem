using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class ManytoManyUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubstationOwners",
                table: "SubstationOwners");

            migrationBuilder.DropIndex(
                name: "IX_SubstationOwners_SubstationId",
                table: "SubstationOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementOwners",
                table: "ElementOwners");

            migrationBuilder.DropIndex(
                name: "IX_ElementOwners_ElementId",
                table: "ElementOwners");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubstationOwners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ElementOwners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubstationOwners",
                table: "SubstationOwners",
                columns: new[] { "SubstationId", "OwnerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementOwners",
                table: "ElementOwners",
                columns: new[] { "ElementId", "OwnerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubstationOwners",
                table: "SubstationOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementOwners",
                table: "ElementOwners");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubstationOwners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ElementOwners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubstationOwners",
                table: "SubstationOwners",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementOwners",
                table: "ElementOwners",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubstationOwners_SubstationId",
                table: "SubstationOwners",
                column: "SubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementOwners_ElementId",
                table: "ElementOwners",
                column: "ElementId");
        }
    }
}
