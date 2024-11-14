using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

/// <inheritdoc />
public partial class nameInCache : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "OwnerIdsCache",
            table: "Substations",
            newName: "OwnerNamesCache");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "OwnerNamesCache",
            table: "Substations",
            newName: "OwnerIdsCache");
    }
}
