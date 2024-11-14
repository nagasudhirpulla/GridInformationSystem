using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

/// <inheritdoc />
public partial class DeleteRestrict : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Bus1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Bus2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_BusId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Element1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Element2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_FilterBankId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_LineId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Line_Bus1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Line_Bus2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Substations_Substation1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Substations_Substation2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Locations_States_StateId",
            table: "Locations");

        migrationBuilder.DropForeignKey(
            name: "FK_States_Regions_RegionId",
            table: "States");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_Fuels_FuelId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_GeneratingStationClassifications_GeneratingStationClassificationId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_GeneratingStationTypes_GeneratingStationTypeId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_Locations_LocationId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_VoltageLevels_VoltageLevelId",
            table: "Substations");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_NameCache",
            table: "Substations",
            column: "NameCache",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GeneratingStationTypes_StationType",
            table: "GeneratingStationTypes",
            column: "StationType",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GeneratingStationClassifications_Classification",
            table: "GeneratingStationClassifications",
            column: "Classification",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Bus1Id",
            table: "Elements",
            column: "Bus1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Bus2Id",
            table: "Elements",
            column: "Bus2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_BusId",
            table: "Elements",
            column: "BusId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Element1Id",
            table: "Elements",
            column: "Element1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Element2Id",
            table: "Elements",
            column: "Element2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_FilterBankId",
            table: "Elements",
            column: "FilterBankId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_LineId",
            table: "Elements",
            column: "LineId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Line_Bus1Id",
            table: "Elements",
            column: "Line_Bus1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Line_Bus2Id",
            table: "Elements",
            column: "Line_Bus2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Substations_Substation1Id",
            table: "Elements",
            column: "Substation1Id",
            principalTable: "Substations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Substations_Substation2Id",
            table: "Elements",
            column: "Substation2Id",
            principalTable: "Substations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Locations_States_StateId",
            table: "Locations",
            column: "StateId",
            principalTable: "States",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_States_Regions_RegionId",
            table: "States",
            column: "RegionId",
            principalTable: "Regions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_Fuels_FuelId",
            table: "Substations",
            column: "FuelId",
            principalTable: "Fuels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_GeneratingStationClassifications_GeneratingStationClassificationId",
            table: "Substations",
            column: "GeneratingStationClassificationId",
            principalTable: "GeneratingStationClassifications",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_GeneratingStationTypes_GeneratingStationTypeId",
            table: "Substations",
            column: "GeneratingStationTypeId",
            principalTable: "GeneratingStationTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_Locations_LocationId",
            table: "Substations",
            column: "LocationId",
            principalTable: "Locations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_VoltageLevels_VoltageLevelId",
            table: "Substations",
            column: "VoltageLevelId",
            principalTable: "VoltageLevels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Bus1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Bus2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_BusId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Element1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Element2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_FilterBankId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_LineId",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Line_Bus1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Elements_Line_Bus2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Substations_Substation1Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Elements_Substations_Substation2Id",
            table: "Elements");

        migrationBuilder.DropForeignKey(
            name: "FK_Locations_States_StateId",
            table: "Locations");

        migrationBuilder.DropForeignKey(
            name: "FK_States_Regions_RegionId",
            table: "States");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_Fuels_FuelId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_GeneratingStationClassifications_GeneratingStationClassificationId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_GeneratingStationTypes_GeneratingStationTypeId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_Locations_LocationId",
            table: "Substations");

        migrationBuilder.DropForeignKey(
            name: "FK_Substations_VoltageLevels_VoltageLevelId",
            table: "Substations");

        migrationBuilder.DropIndex(
            name: "IX_Substations_NameCache",
            table: "Substations");

        migrationBuilder.DropIndex(
            name: "IX_GeneratingStationTypes_StationType",
            table: "GeneratingStationTypes");

        migrationBuilder.DropIndex(
            name: "IX_GeneratingStationClassifications_Classification",
            table: "GeneratingStationClassifications");

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Bus1Id",
            table: "Elements",
            column: "Bus1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Bus2Id",
            table: "Elements",
            column: "Bus2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_BusId",
            table: "Elements",
            column: "BusId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Element1Id",
            table: "Elements",
            column: "Element1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Element2Id",
            table: "Elements",
            column: "Element2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_FilterBankId",
            table: "Elements",
            column: "FilterBankId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_LineId",
            table: "Elements",
            column: "LineId",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Line_Bus1Id",
            table: "Elements",
            column: "Line_Bus1Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Elements_Line_Bus2Id",
            table: "Elements",
            column: "Line_Bus2Id",
            principalTable: "Elements",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Substations_Substation1Id",
            table: "Elements",
            column: "Substation1Id",
            principalTable: "Substations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Elements_Substations_Substation2Id",
            table: "Elements",
            column: "Substation2Id",
            principalTable: "Substations",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Locations_States_StateId",
            table: "Locations",
            column: "StateId",
            principalTable: "States",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_States_Regions_RegionId",
            table: "States",
            column: "RegionId",
            principalTable: "Regions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_Fuels_FuelId",
            table: "Substations",
            column: "FuelId",
            principalTable: "Fuels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_GeneratingStationClassifications_GeneratingStationClassificationId",
            table: "Substations",
            column: "GeneratingStationClassificationId",
            principalTable: "GeneratingStationClassifications",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_GeneratingStationTypes_GeneratingStationTypeId",
            table: "Substations",
            column: "GeneratingStationTypeId",
            principalTable: "GeneratingStationTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_Locations_LocationId",
            table: "Substations",
            column: "LocationId",
            principalTable: "Locations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Substations_VoltageLevels_VoltageLevelId",
            table: "Substations",
            column: "VoltageLevelId",
            principalTable: "VoltageLevels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
