using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations;

/// <inheritdoc />
public partial class init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Fuels",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FuelName = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Fuels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GeneratingStationClassifications",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Classification = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GeneratingStationClassifications", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GeneratingStationTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                StationType = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GeneratingStationTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Owners",
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
                table.PrimaryKey("PK_Owners", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Regions",
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
                table.PrimaryKey("PK_Regions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "VoltageLevels",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Level = table.Column<string>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VoltageLevels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RoleId = table.Column<string>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                RoleId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "States",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                RegionId = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_States", x => x.Id);
                table.ForeignKey(
                    name: "FK_States_Regions_RegionId",
                    column: x => x.RegionId,
                    principalTable: "Regions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Locations",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Alias = table.Column<string>(type: "TEXT", nullable: true),
                RegionId = table.Column<int>(type: "INTEGER", nullable: false),
                StateId = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Locations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Locations_Regions_RegionId",
                    column: x => x.RegionId,
                    principalTable: "Regions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Locations_States_StateId",
                    column: x => x.StateId,
                    principalTable: "States",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Substations",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NameCache = table.Column<string>(type: "TEXT", nullable: false),
                OwnerNamesCache = table.Column<string>(type: "TEXT", nullable: false),
                VoltageLevelId = table.Column<int>(type: "INTEGER", nullable: false),
                LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                IsAc = table.Column<bool>(type: "INTEGER", nullable: false),
                Latitude = table.Column<double>(type: "REAL", nullable: false),
                Longitude = table.Column<double>(type: "REAL", nullable: false),
                Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: true),
                Installedcapacity = table.Column<double>(type: "REAL", nullable: true),
                MVAcapacity = table.Column<double>(type: "REAL", nullable: true),
                GeneratingStationClassificationId = table.Column<int>(type: "INTEGER", nullable: true),
                GeneratingStationTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                FuelId = table.Column<int>(type: "INTEGER", nullable: true),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Substations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Substations_Fuels_FuelId",
                    column: x => x.FuelId,
                    principalTable: "Fuels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Substations_GeneratingStationClassifications_GeneratingStationClassificationId",
                    column: x => x.GeneratingStationClassificationId,
                    principalTable: "GeneratingStationClassifications",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Substations_GeneratingStationTypes_GeneratingStationTypeId",
                    column: x => x.GeneratingStationTypeId,
                    principalTable: "GeneratingStationTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Substations_Locations_LocationId",
                    column: x => x.LocationId,
                    principalTable: "Locations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Substations_VoltageLevels_VoltageLevelId",
                    column: x => x.VoltageLevelId,
                    principalTable: "VoltageLevels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Elements",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ElementNameCache = table.Column<int>(type: "INTEGER", nullable: false),
                VoltLevelCache = table.Column<string>(type: "TEXT", nullable: false),
                Substation1Id = table.Column<int>(type: "INTEGER", nullable: false),
                SubstationId1 = table.Column<int>(type: "INTEGER", nullable: false),
                Substation2Id = table.Column<int>(type: "INTEGER", nullable: true),
                SubstationId2 = table.Column<int>(type: "INTEGER", nullable: true),
                OwnerNamesCache = table.Column<string>(type: "TEXT", nullable: false),
                ElementNumber = table.Column<string>(type: "TEXT", nullable: false),
                CommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                DeCommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                CommercialOperationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                IsImportantGridElement = table.Column<bool>(type: "INTEGER", nullable: false),
                Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                Element1Id = table.Column<int>(type: "INTEGER", nullable: true),
                Element2Id = table.Column<int>(type: "INTEGER", nullable: true),
                BayType = table.Column<int>(type: "INTEGER", nullable: true),
                IsFuture = table.Column<bool>(type: "INTEGER", nullable: true),
                BusType = table.Column<int>(type: "INTEGER", nullable: true),
                BusId = table.Column<int>(type: "INTEGER", nullable: true),
                MvarCapacity = table.Column<double>(type: "REAL", nullable: true),
                Mvar = table.Column<double>(type: "REAL", nullable: true),
                IsSwitchable = table.Column<bool>(type: "INTEGER", nullable: true),
                InstalledCapacity = table.Column<double>(type: "REAL", nullable: true),
                GeneratingVoltage = table.Column<double>(type: "REAL", nullable: true),
                Bus1Id = table.Column<int>(type: "INTEGER", nullable: true),
                Bus2Id = table.Column<int>(type: "INTEGER", nullable: true),
                Length = table.Column<double>(type: "REAL", nullable: true),
                ConductorType = table.Column<string>(type: "TEXT", nullable: true),
                IsSpsPresent = table.Column<bool>(type: "INTEGER", nullable: true),
                PoleType = table.Column<int>(type: "INTEGER", nullable: true),
                Line_Bus1Id = table.Column<int>(type: "INTEGER", nullable: true),
                Line_Bus2Id = table.Column<int>(type: "INTEGER", nullable: true),
                Line_Length = table.Column<double>(type: "REAL", nullable: true),
                Line_ConductorType = table.Column<string>(type: "TEXT", nullable: true),
                IsAutoReclosurePresent = table.Column<bool>(type: "INTEGER", nullable: true),
                LineId = table.Column<int>(type: "INTEGER", nullable: true),
                LineReactor_MvarCapacity = table.Column<double>(type: "REAL", nullable: true),
                IsConvertible = table.Column<bool>(type: "INTEGER", nullable: true),
                LineReactor_IsSwitchable = table.Column<bool>(type: "INTEGER", nullable: true),
                FilterBankId = table.Column<int>(type: "INTEGER", nullable: true),
                SubFilterTag = table.Column<string>(type: "TEXT", nullable: true),
                SubFilterBank_Mvar = table.Column<double>(type: "REAL", nullable: true),
                SubFilterBank_IsSwitchable = table.Column<bool>(type: "INTEGER", nullable: true),
                TransformerType = table.Column<int>(type: "INTEGER", nullable: true),
                MvaCapacity = table.Column<double>(type: "REAL", nullable: true),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Elements", x => x.Id);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Bus1Id",
                    column: x => x.Bus1Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Bus2Id",
                    column: x => x.Bus2Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_BusId",
                    column: x => x.BusId,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Element1Id",
                    column: x => x.Element1Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Element2Id",
                    column: x => x.Element2Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_FilterBankId",
                    column: x => x.FilterBankId,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_LineId",
                    column: x => x.LineId,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Line_Bus1Id",
                    column: x => x.Line_Bus1Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Elements_Line_Bus2Id",
                    column: x => x.Line_Bus2Id,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Locations_LocationId",
                    column: x => x.LocationId,
                    principalTable: "Locations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Substations_Substation1Id",
                    column: x => x.Substation1Id,
                    principalTable: "Substations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Elements_Substations_Substation2Id",
                    column: x => x.Substation2Id,
                    principalTable: "Substations",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "SubstationOwners",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                SubstationId = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubstationOwners", x => x.Id);
                table.ForeignKey(
                    name: "FK_SubstationOwners_Owners_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Owners",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SubstationOwners_Substations_SubstationId",
                    column: x => x.SubstationId,
                    principalTable: "Substations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ElementOwners",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                ElementId = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElementOwners", x => x.Id);
                table.ForeignKey(
                    name: "FK_ElementOwners_Elements_ElementId",
                    column: x => x.ElementId,
                    principalTable: "Elements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ElementOwners_Owners_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Owners",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ElementOwners_ElementId",
            table: "ElementOwners",
            column: "ElementId");

        migrationBuilder.CreateIndex(
            name: "IX_ElementOwners_OwnerId",
            table: "ElementOwners",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Bus1Id",
            table: "Elements",
            column: "Bus1Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Bus2Id",
            table: "Elements",
            column: "Bus2Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_BusId",
            table: "Elements",
            column: "BusId");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Element1Id",
            table: "Elements",
            column: "Element1Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Element2Id",
            table: "Elements",
            column: "Element2Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_ElementNameCache",
            table: "Elements",
            column: "ElementNameCache",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Elements_FilterBankId",
            table: "Elements",
            column: "FilterBankId");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Line_Bus1Id",
            table: "Elements",
            column: "Line_Bus1Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Line_Bus2Id",
            table: "Elements",
            column: "Line_Bus2Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_LineId",
            table: "Elements",
            column: "LineId");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_LocationId",
            table: "Elements",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Substation1Id",
            table: "Elements",
            column: "Substation1Id");

        migrationBuilder.CreateIndex(
            name: "IX_Elements_Substation2Id",
            table: "Elements",
            column: "Substation2Id");

        migrationBuilder.CreateIndex(
            name: "IX_Fuels_FuelName",
            table: "Fuels",
            column: "FuelName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Locations_Name",
            table: "Locations",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Locations_RegionId",
            table: "Locations",
            column: "RegionId");

        migrationBuilder.CreateIndex(
            name: "IX_Locations_StateId",
            table: "Locations",
            column: "StateId");

        migrationBuilder.CreateIndex(
            name: "IX_Owners_Name",
            table: "Owners",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Regions_Name",
            table: "Regions",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_States_Name",
            table: "States",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_States_RegionId",
            table: "States",
            column: "RegionId");

        migrationBuilder.CreateIndex(
            name: "IX_SubstationOwners_OwnerId",
            table: "SubstationOwners",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_SubstationOwners_SubstationId",
            table: "SubstationOwners",
            column: "SubstationId");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_FuelId",
            table: "Substations",
            column: "FuelId");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_GeneratingStationClassificationId",
            table: "Substations",
            column: "GeneratingStationClassificationId");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_GeneratingStationTypeId",
            table: "Substations",
            column: "GeneratingStationTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_LocationId",
            table: "Substations",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_Substations_Name",
            table: "Substations",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Substations_VoltageLevelId",
            table: "Substations",
            column: "VoltageLevelId");

        migrationBuilder.CreateIndex(
            name: "IX_VoltageLevels_Level",
            table: "VoltageLevels",
            column: "Level",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "ElementOwners");

        migrationBuilder.DropTable(
            name: "SubstationOwners");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "Elements");

        migrationBuilder.DropTable(
            name: "Owners");

        migrationBuilder.DropTable(
            name: "Substations");

        migrationBuilder.DropTable(
            name: "Fuels");

        migrationBuilder.DropTable(
            name: "GeneratingStationClassifications");

        migrationBuilder.DropTable(
            name: "GeneratingStationTypes");

        migrationBuilder.DropTable(
            name: "Locations");

        migrationBuilder.DropTable(
            name: "VoltageLevels");

        migrationBuilder.DropTable(
            name: "States");

        migrationBuilder.DropTable(
            name: "Regions");
    }
}
