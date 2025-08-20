using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class MeasurementKeyCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Measurements",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_EntityId_MetricId_DatasourceId",
                table: "Measurements",
                columns: new[] { "EntityId", "MetricId", "DatasourceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_EntityId_MetricId_DatasourceId",
                table: "Measurements");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Measurements",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements",
                columns: new[] { "EntityId", "MetricId", "DatasourceId" });
        }
    }
}
