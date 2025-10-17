﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiRoles",
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
                    table.PrimaryKey("PK_ApiRoles", x => x.Id);
                });

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
                name: "Datasources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    BaseUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: true),
                    PayloadSchema = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datasources", x => x.Id);
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
                name: "Tags",
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
                    table.PrimaryKey("PK_Tags", x => x.Id);
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
                name: "ApiClientRoles",
                columns: table => new
                {
                    ApiClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApiRoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClientRoles", x => new { x.ApiClientId, x.ApiRoleId });
                    table.ForeignKey(
                        name: "FK_ApiClientRoles_ApiClients_ApiClientId",
                        column: x => x.ApiClientId,
                        principalTable: "ApiClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiClientRoles_ApiRoles_ApiRoleId",
                        column: x => x.ApiRoleId,
                        principalTable: "ApiRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "GridEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    VoltageLevelCache = table.Column<string>(type: "TEXT", nullable: true),
                    Element_RegionCache = table.Column<string>(type: "TEXT", nullable: true),
                    Substation1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Substation2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Element_OwnerNamesCache = table.Column<string>(type: "TEXT", nullable: true),
                    ElementNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeCommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CommercialOperationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsImportantGridElement = table.Column<bool>(type: "INTEGER", nullable: true),
                    Element1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Element2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    BayType = table.Column<int>(type: "INTEGER", nullable: true),
                    IsFuture = table.Column<bool>(type: "INTEGER", nullable: true),
                    BusType = table.Column<int>(type: "INTEGER", nullable: true),
                    BusId = table.Column<int>(type: "INTEGER", nullable: true),
                    MvarCapacity = table.Column<double>(type: "REAL", nullable: true),
                    Mvar = table.Column<double>(type: "REAL", nullable: true),
                    IsSwitchable = table.Column<bool>(type: "INTEGER", nullable: true),
                    Capacity = table.Column<double>(type: "REAL", nullable: true),
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
                    SubFilterBank_Mvar = table.Column<double>(type: "REAL", nullable: true),
                    SubFilterBank_IsSwitchable = table.Column<bool>(type: "INTEGER", nullable: true),
                    TransformerType = table.Column<int>(type: "INTEGER", nullable: true),
                    Transformer_MvaCapacity = table.Column<double>(type: "REAL", nullable: true),
                    Make = table.Column<string>(type: "TEXT", nullable: true),
                    Alias = table.Column<string>(type: "TEXT", nullable: true),
                    Location_RegionCache = table.Column<string>(type: "TEXT", nullable: true),
                    StateId = table.Column<int>(type: "INTEGER", nullable: true),
                    RegionId = table.Column<int>(type: "INTEGER", nullable: true),
                    OwnerNamesCache = table.Column<string>(type: "TEXT", nullable: true),
                    RegionCache = table.Column<string>(type: "TEXT", nullable: true),
                    VoltageLevelId = table.Column<int>(type: "INTEGER", nullable: true),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAc = table.Column<bool>(type: "INTEGER", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    InstalledCapacity = table.Column<double>(type: "REAL", nullable: true),
                    MvaCapacity = table.Column<double>(type: "REAL", nullable: true),
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
                    table.PrimaryKey("PK_GridEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridEntities_GeneratingStationClassifications_GeneratingStationClassificationId",
                        column: x => x.GeneratingStationClassificationId,
                        principalTable: "GeneratingStationClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GeneratingStationTypes_GeneratingStationTypeId",
                        column: x => x.GeneratingStationTypeId,
                        principalTable: "GeneratingStationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Bus1Id",
                        column: x => x.Bus1Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Bus2Id",
                        column: x => x.Bus2Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_BusId",
                        column: x => x.BusId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Element1Id",
                        column: x => x.Element1Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Element2Id",
                        column: x => x.Element2Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_FilterBankId",
                        column: x => x.FilterBankId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_FuelId",
                        column: x => x.FuelId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_LineId",
                        column: x => x.LineId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Line_Bus1Id",
                        column: x => x.Line_Bus1Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Line_Bus2Id",
                        column: x => x.Line_Bus2Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_LocationId",
                        column: x => x.LocationId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_RegionId",
                        column: x => x.RegionId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_StateId",
                        column: x => x.StateId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Substation1Id",
                        column: x => x.Substation1Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_GridEntities_Substation2Id",
                        column: x => x.Substation2Id,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridEntities_VoltageLevels_VoltageLevelId",
                        column: x => x.VoltageLevelId,
                        principalTable: "VoltageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElementOwners",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ElementId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementOwners", x => new { x.ElementId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_ElementOwners_GridEntities_ElementId",
                        column: x => x.ElementId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementOwners_GridEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GridEntityTags",
                columns: table => new
                {
                    GridEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridEntityTags", x => new { x.GridEntityId, x.TagId });
                    table.ForeignKey(
                        name: "FK_GridEntityTags_GridEntities_GridEntityId",
                        column: x => x.GridEntityId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GridEntityTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    DatasourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    HistorianPntId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Datasources_DatasourceId",
                        column: x => x.DatasourceId,
                        principalTable: "Datasources",
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

            migrationBuilder.CreateTable(
                name: "SubstationOwners",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubstationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstationOwners", x => new { x.SubstationId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_SubstationOwners_GridEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubstationOwners_GridEntities_SubstationId",
                        column: x => x.SubstationId,
                        principalTable: "GridEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiClientRoles_ApiRoleId",
                table: "ApiClientRoles",
                column: "ApiRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiClients_Key",
                table: "ApiClients",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiClients_Name",
                table: "ApiClients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiRoles_Name",
                table: "ApiRoles",
                column: "Name",
                unique: true);

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
                name: "IX_Datasources_Name",
                table: "Datasources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElementOwners_OwnerId",
                table: "ElementOwners",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratingStationClassifications_Classification",
                table: "GeneratingStationClassifications",
                column: "Classification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneratingStationTypes_StationType",
                table: "GeneratingStationTypes",
                column: "StationType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Bus1Id",
                table: "GridEntities",
                column: "Bus1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Bus2Id",
                table: "GridEntities",
                column: "Bus2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_BusId",
                table: "GridEntities",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Element1Id",
                table: "GridEntities",
                column: "Element1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Element2Id",
                table: "GridEntities",
                column: "Element2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_FilterBankId",
                table: "GridEntities",
                column: "FilterBankId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_FuelId",
                table: "GridEntities",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_GeneratingStationClassificationId",
                table: "GridEntities",
                column: "GeneratingStationClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_GeneratingStationTypeId",
                table: "GridEntities",
                column: "GeneratingStationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Line_Bus1Id",
                table: "GridEntities",
                column: "Line_Bus1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Line_Bus2Id",
                table: "GridEntities",
                column: "Line_Bus2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_LineId",
                table: "GridEntities",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_LocationId",
                table: "GridEntities",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Name",
                table: "GridEntities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_RegionId",
                table: "GridEntities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_StateId",
                table: "GridEntities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Substation1Id",
                table: "GridEntities",
                column: "Substation1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_Substation2Id",
                table: "GridEntities",
                column: "Substation2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntities_VoltageLevelId",
                table: "GridEntities",
                column: "VoltageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GridEntityTags_TagId",
                table: "GridEntityTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_DatasourceId",
                table: "Measurements",
                column: "DatasourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_EntityId_MetricId_DatasourceId",
                table: "Measurements",
                columns: new[] { "EntityId", "MetricId", "DatasourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MetricId",
                table: "Measurements",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_Name",
                table: "Metrics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstationOwners_OwnerId",
                table: "SubstationOwners",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

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
                name: "ApiClientRoles");

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
                name: "GridEntityTags");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "SubstationOwners");

            migrationBuilder.DropTable(
                name: "ApiClients");

            migrationBuilder.DropTable(
                name: "ApiRoles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Datasources");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "GridEntities");

            migrationBuilder.DropTable(
                name: "GeneratingStationClassifications");

            migrationBuilder.DropTable(
                name: "GeneratingStationTypes");

            migrationBuilder.DropTable(
                name: "VoltageLevels");
        }
    }
}
