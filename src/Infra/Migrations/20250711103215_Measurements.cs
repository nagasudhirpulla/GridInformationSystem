using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

/// <inheritdoc />
public partial class Measurements : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DataSources",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DataSources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Metrics",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Unit = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Metrics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Measurements",
            columns: table => new
            {
                EntityId = table.Column<int>(type: "INTEGER", nullable: false),
                MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                DataSourceId = table.Column<int>(type: "INTEGER", nullable: false),
                HistorianPntId = table.Column<string>(type: "TEXT", nullable: false),
                Id = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Measurements", x => new { x.EntityId, x.MetricId, x.DataSourceId });
                table.ForeignKey(
                    name: "FK_Measurements_DataSources_DataSourceId",
                    column: x => x.DataSourceId,
                    principalTable: "DataSources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Measurements_GridEntities_EntityId",
                    column: x => x.EntityId,
                    principalTable: "GridEntities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Measurements_Metrics_MetricId",
                    column: x => x.MetricId,
                    principalTable: "Metrics",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DataSources_Name",
            table: "DataSources",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Measurements_DataSourceId",
            table: "Measurements",
            column: "DatasourceId");

        migrationBuilder.CreateIndex(
            name: "IX_Measurements_MetricId",
            table: "Measurements",
            column: "MetricId");

        migrationBuilder.CreateIndex(
            name: "IX_Metrics_Name",
            table: "Metrics",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Measurements");

        migrationBuilder.DropTable(
            name: "DataSources");

        migrationBuilder.DropTable(
            name: "Metrics");
    }
}
