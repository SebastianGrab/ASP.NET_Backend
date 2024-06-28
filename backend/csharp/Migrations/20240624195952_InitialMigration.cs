using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    OrganizationType = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    LastPasswordChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TemplateContent = table.Column<string>(type: "text", nullable: false),
                    organizationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Organizations_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    LastLoginAttempt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginAttempts_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    MessageContent = table.Column<string>(type: "text", nullable: false),
                    ReferenceObject = table.Column<string>(type: "text", nullable: true),
                    ReferenceObjectId = table.Column<long>(type: "bigint", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentFrom = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganizationRoles",
                columns: table => new
                {
                    organizationId = table.Column<long>(type: "bigint", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    roleId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganizationRoles", x => new { x.userId, x.roleId, x.organizationId });
                    table.ForeignKey(
                        name: "FK_UserOrganizationRoles_Organizations_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganizationRoles_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganizationRoles_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDraft = table.Column<bool>(type: "boolean", nullable: false),
                    IsInReview = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewComment = table.Column<string>(type: "text", nullable: true),
                    ReviewCommentIsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sendEmail = table.Column<bool>(type: "boolean", nullable: true),
                    emailSubject = table.Column<string>(type: "text", nullable: true),
                    emailContent = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TemplateId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Protocols_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Protocols_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Protocols_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateOrganizations",
                columns: table => new
                {
                    organizationId = table.Column<long>(type: "bigint", nullable: false),
                    templateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateOrganizations", x => new { x.organizationId, x.templateId });
                    table.ForeignKey(
                        name: "FK_TemplateOrganizations_Organizations_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateOrganizations_Templates_templateId",
                        column: x => x.templateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateVersions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateContent = table.Column<string>(type: "text", nullable: false),
                    templateId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Templates_templateId",
                        column: x => x.templateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalUsers",
                columns: table => new
                {
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    protocolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalUsers", x => new { x.userId, x.protocolId });
                    table.ForeignKey(
                        name: "FK_AdditionalUsers_Protocols_protocolId",
                        column: x => x.protocolId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalUsers_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolContents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    protocolId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtocolContents_Protocols_protocolId",
                        column: x => x.protocolId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolPdfFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    protocolId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolPdfFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtocolPdfFiles_Protocols_protocolId",
                        column: x => x.protocolId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "City", "CreatedDate", "Name", "OrganizationType", "PostalCode", "UpdatedDate", "parentId" },
                values: new object[] { -1L, "Carstennstraße 58", "Berlin", new DateTime(2024, 6, 24, 19, 59, 51, 616, DateTimeKind.Utc).AddTicks(3779), "Deutsches Rotes Kreuz e.V.", "Bundesorganisation", "12205", new DateTime(2024, 6, 24, 19, 59, 51, 616, DateTimeKind.Utc).AddTicks(3781), null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -3L, "Admin" },
                    { -2L, "Leiter" },
                    { -1L, "Helfer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "LastPasswordChangeDate", "Password", "UpdatedDate" },
                values: new object[] { -1L, new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(4509), "superadmin@drk.de", "Super", "Admin", new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(4516), "$2a$11$jfOmSzjwV6H84Y1IlFUYNuddvPkELdgShaYycWe9xgxi3u3XEh1xO", new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(4515) });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "TemplateContent", "UpdatedDate", "organizationId" },
                values: new object[] { -1L, new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(8200), "Standard-Template für alle Organisationen", "Standard-Template", "{\"Name\":\"Protokollschema\",\"Schema\":[{\"Kategorie\":\"Schlüssel\",\"ID\":\"SCHLUESSEL\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Alarmschluessel\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Alarmschlüssel:\",\"ID\":\"ALARMSCHLUESSEL\",\"Mandatory\":1,\"Pattern\":\"^[123][0-9]{3}[NBnb]?$\",\"Placeholder\":\"1000N\"},{\"Name\":\"Auftragsnummer\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"Auftrags Nr:\",\"ID\":\"AUFTRAGSNUMMER\",\"Mandatory\":1,\"DisableHandlerId\":\"Auftragsnummer-Mandatoryhandler\",\"Pattern\":\"^([0-9]+)$\",\"Placeholder\":\"123456\"},{\"Name\":\"Auftragsnummer-Mandatoryhandler\",\"Element\":\"mandatoryhandler\",\"Type\":\"mandatoryhandler\",\"Label\":\"Keine Auftragsnummer:\",\"ID\":\"Auftragsnummer-Mandatoryhandler\"}]},{\"Kategorie\":\"Einsatzort\",\"ID\":\"EINSATZORT-KATEGORIE\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Datum\",\"Element\":\"input\",\"Type\":\"date\",\"Label\":\"Datum:\",\"Mandatory\":1,\"ID\":\"DATUM\"},{\"Name\":\"Ort\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Einsatzort:\",\"Mandatory\":1,\"ID\":\"EINSATZORT\",\"Placeholder\":\"Stuttgart\"},{\"Name\":\"Strasse\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Straße:\",\"Mandatory\":1,\"ID\":\"EINSATSSTRASSE\",\"Placeholder\":\"Hauptstraße\"},{\"Name\":\"PLZ\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"PLZ:\",\"ID\":\"EINSATZPLZ\"},{\"Name\":\"Alarmzeit\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Alarmzeit:\",\"ID\":\"ALARMZEIT\"},{\"Name\":\"Ankunft_HvO\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Ankunft HvO:\",\"ID\":\"ANKUNFT_HVO\"},{\"Name\":\"Einsatzende_HvO\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Einsatzende HvO:\",\"ID\":\"EINSATZENDE_HVO\"},{\"Name\":\"RTW_NEF\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RTW/NEF:\",\"ID\":\"RTW_NEF\"},{\"Name\":\"Ankunft_RTW_NEF\",\"Element\":\"input\",\"Type\":\"time\",\"Label\":\"Ankunft RTW/NEF:\",\"ID\":\"ANKUNFT_RTW_NEF\"}]},{\"Kategorie\":\"Fahrzeug\",\"ID\":\"FAHRZEUG\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Einsatzfahrzeug\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Einsatzfahrzeug:\",\"ID\":\"EINSATZFAHRZEUG\",\"Placeholder\":\"x/xx/x\"},{\"Name\":\"RD\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"RD:\",\"ID\":\"RD\",\"Placeholder\":\"RTW\"},{\"Name\":\"Privat_PKW\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Privat PKW:\",\"ID\":\"PRIVAT_PKW\"},{\"Name\":\"Liste_Privat_PKW\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Weitere private PKW:\",\"Placeholder\":\"x-xx-xxxx\",\"ID\":\"LISTE_PRIVAT_PKW\"}]},{\"Kategorie\":\"Einsatzhelfer\",\"ID\":\"EINSATZHELFER\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Helfer\",\"Element\":\"dropdownHelper\",\"Type\":\"dropdown\",\"Options\":\"/Data/DropdownHelfer.json\",\"Label\":\"Helfer:\",\"HelperCollection\":[\"HELFERNAMENDD1\"],\"HelperNames\":[\"\"],\"ID\":\"HELFERNAMENDD\",\"Location\":\"beim Patient\"}]},{\"Kategorie\":\"Atemwege\",\"ID\":\"ATEMWEGE\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Frei\",\"Element\":\"input\",\"Type\":\"radio\",\"RadioGroup\":\"atemwege\",\"Label\":\"frei:\",\"ID\":\"FREI\"},{\"Name\":\"Verlegt\",\"Element\":\"input\",\"Type\":\"radio\",\"RadioGroup\":\"atemwege\",\"Label\":\"verlegt:\",\"ID\":\"VERLEGT\"},{\"Name\":\"Atemweg_Zusatz\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Sonstiges:\",\"ID\":\"ATEMWEGZUSATZ\",\"Placeholder\":\"z.B. verlegtdurch ...\"}]},{\"Kategorie\":\"Belüftung\",\"ID\":\"BELUEFTUNG\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"unauffaellig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unauffällig:\",\"ID\":\"UNAUFFAELLIG\"},{\"Name\":\"Zyanose\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Zyanose:\",\"ID\":\"ZYANOSE\"},{\"Name\":\"Rasseln\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Rasseln:\",\"ID\":\"RASSELN\"},{\"Name\":\"Schnappatmung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Schnappatmung:\",\"ID\":\"SCHNAPPATMUNG\"},{\"Name\":\"Atemnot\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemnot:\",\"ID\":\"ATEMNOT\"},{\"Name\":\"Hyperventilation\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Hyperventilation:\",\"ID\":\"HYPERVENTILATION\"},{\"Name\":\"Atemstillstand\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemstillstand:\",\"ID\":\"ATEMSTILLSTAND\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"}]},{\"Kategorie\":\"Circulation\",\"ID\":\"CIRCULATION\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Puls_Label\",\"Element\":\"label\",\"Label\":\"Puls:\",\"ID\":\"PULS_LABEL\"},{\"Name\":\"regelmaeßig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"regelmäßig:\",\"ID\":\"REGELMAESSIG\"},{\"Name\":\"unregelmaeßig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unregelmäßig:\",\"ID\":\"UNREGELMAESSIG\"},{\"Name\":\"gut_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"gut tastbar:\",\"ID\":\"GUT_TASTBAR\"},{\"Name\":\"schlecht_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"schlecht tastbar:\",\"ID\":\"SCHLECHT_TASTBAR\"},{\"Name\":\"nicht_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"nicht tastbar:\",\"ID\":\"NICHT_TASTBAR\"},{\"Name\":\"Haut_Label\",\"Element\":\"label\",\"Label\":\"Haut:\",\"ID\":\"HAUT_LABEL\"},{\"Name\":\"rosig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"rosig:\",\"ID\":\"ROSIG\"},{\"Name\":\"blass\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"blass:\",\"ID\":\"BLASS\"},{\"Name\":\"blau\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"blau:\",\"ID\":\"BLAU\"},{\"Name\":\"rot\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"rot:\",\"ID\":\"ROT\"},{\"Name\":\"warm\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"warm:\",\"ID\":\"WARM\"},{\"Name\":\"kalt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"kalt:\",\"ID\":\"KALT\"},{\"Name\":\"CWERTE_Label\",\"Element\":\"label\",\"Label\":\"Werte:\",\"ID\":\"CWERTE_LABEL\"},{\"Name\":\"Puls\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"Puls:\",\"ID\":\"PULS\",\"Mandatory\":1},{\"Name\":\"RR_sys\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RR sys.:\",\"ID\":\"RR_SYS\"},{\"Name\":\"RR_dia\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RR dia.:\",\"ID\":\"RR_DIA\"},{\"Name\":\"SPO2\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"SPO²:\",\"ID\":\"SPO2\"},{\"Name\":\"keine_Werte\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"keine Werte:\",\"ID\":\"KEINE_WERTE\"}]},{\"Kategorie\":\"Defizit\",\"ID\":\"DEFIZIT\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Bewusstein\",\"Element\":\"label\",\"Label\":\"Bewusstein:\",\"ID\":\"BEWUSSTEIN\"},{\"Name\":\"orientiert\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"orientiert:\",\"ID\":\"ORIENTIERT\"},{\"Name\":\"desorientiert\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"desorientiert:\",\"ID\":\"DESORIENTIERT\"},{\"Name\":\"getrübt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"getrübt:\",\"ID\":\"GETRÜBT\"},{\"Name\":\"bewusstlos\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"bewusstlos:\",\"ID\":\"BEWUSSTLOS\"},{\"Name\":\"BZ\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"BZ:\",\"ID\":\"BZ\"},{\"Name\":\"Pupillen\",\"Element\":\"label\",\"Label\":\"Pupillen:\",\"ID\":\"PUPILLEN\"},{\"Name\":\"links\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"eng\",\"weit\",\"entrundet\"],\"Label\":\"links:\",\"ID\":\"LINKS\"},{\"Name\":\"rechts\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"eng\",\"weit\",\"entrundet\"],\"Label\":\"rechts:\",\"ID\":\"RECHTS\"},{\"Name\":\"keine_Lichtreflexe\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"links\",\"rechts\",\"beidseitig\"],\"Label\":\"keine Lichtreflexe:\",\"ID\":\"KEINE_LICHTREFLEXE\"},{\"Name\":\"Schmerzen\",\"Element\":\"label\",\"Label\":\"Schmerzen:\",\"ID\":\"SCHMERZEN\"},{\"Name\":\"keine\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"keine:\",\"ID\":\"KEINE\"},{\"Name\":\"leicht\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"leicht:\",\"ID\":\"LEICHT\"},{\"Name\":\"mittel\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"mittel:\",\"ID\":\"MITTEL\"},{\"Name\":\"stark\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"stark:\",\"ID\":\"STARK\"},{\"Name\":\"kolikartig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"kolikartig:\",\"ID\":\"KOLIKARTIG\"},{\"Name\":\"Schmerz_index\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"],\"Label\":\"Index 1 bis 10:\",\"ID\":\"SCHMERZ_INDEX\"}]},{\"Kategorie\":\"Verletzungen\",\"ID\":\"VERLETZUNGEN\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"unverletzt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unverletzt:\",\"ID\":\"UNVERLETZT\"},{\"Name\":\"Schädel_Hirn\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Schädel Hirn:\",\"ID\":\"SCHÄDEL_HIRN\"},{\"Name\":\"Gesicht\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Gesicht:\",\"ID\":\"GESICHT\"},{\"Name\":\"HWS\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"HWS:\",\"ID\":\"HWS\"},{\"Name\":\"Thorax\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Thorax:\",\"ID\":\"THORAX\"},{\"Name\":\"Abdomen\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Abdomen:\",\"ID\":\"ABDOMEN\"},{\"Name\":\"BWS_LWS\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"BWS/LWS:\",\"ID\":\"BWS_LWS\"},{\"Name\":\"Becken\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Becken:\",\"ID\":\"BECKEN\"},{\"Name\":\"Obere_Extremität\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Obere Extremität:\",\"ID\":\"OBERE_EXTREMITÄT\"},{\"Name\":\"Untere_Extremität\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Untere Extremität:\",\"ID\":\"UNTERE_EXTREMITÄT\"},{\"Name\":\"Weichteile\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Weichteile:\",\"ID\":\"WEICHTEILE\"}]},{\"Kategorie\":\"Maßnahmen\",\"ID\":\"MASSNAHMEN\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Atemwege_freimachen\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemwege freimachen:\",\"ID\":\"ATEMWEGE_FREIMACHEN\"},{\"Name\":\"Larynxtubus\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Larynxtubus:\",\"ID\":\"LARYNXTUBUS\"},{\"Name\":\"O2_Gabe\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"O2 Gabe:\",\"ID\":\"O2_GABE\"},{\"Name\":\"l_min\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"l/min:\",\"ID\":\"L_MIN\",\"Pattern\":\"\\b([1-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|250)\\b\",\"Placeholder\":\"5\"},{\"Name\":\"Brille_Maske_Beutel\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Brille/Maske/Beutel:\",\"ID\":\"BRILLE_MASKE_BEUTEL\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"},{\"Name\":\"Herzdruckmassage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Herzdruckmassage:\",\"ID\":\"HERZDRUCKMASSAGE\"},{\"Name\":\"AED\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"AED:\",\"ID\":\"AED\"},{\"Name\":\"Defibrilliert\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Defibrilliert:\",\"ID\":\"DEFIBRILLIERT\"},{\"Name\":\"Wundversorgung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Wundversorgung:\",\"ID\":\"WUNDVERSORGUNG\"},{\"Name\":\"HWS_Fixierung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"HWS Fixierung:\",\"ID\":\"HWS_FIXIERUNG\"},{\"Name\":\"NA_Nachforderung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"NA Nachforderung:\",\"ID\":\"NA_NACHFORDERUNG\"},{\"Name\":\"Seitenlage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Seitenlage:\",\"ID\":\"SEITENLAGE\"},{\"Name\":\"Oberkoerper_hoch_sitzend\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Oberkörper hoch/sitzend:\",\"ID\":\"OBERKOERPER_HOCH_SITZEND\"},{\"Name\":\"Flachlagerung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Flachlagerung:\",\"ID\":\"FLACHLAGERUNG\"},{\"Name\":\"Schocklage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Schocklage:\",\"ID\":\"SCHOCKLAGE\"},{\"Name\":\"Ruhigstellung_mit\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Ruhigstellung mit:\",\"ID\":\"RUHIGSTELLUNG_MIT\",\"Pattern\":\"^[a-zA-Z]+$\",\"Placeholder\":\"z.B. Schiene\"},{\"Name\":\"Absicherung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Absicherung:\",\"ID\":\"ABSICHERUNG\"},{\"Name\":\"Einweisung_RD\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Einweisung RD:\",\"ID\":\"EINWEISUNG_RD\"},{\"Name\":\"Unterstuetzung_RD\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Unterstützung RD:\",\"ID\":\"UNTERSTUETZUNG_RD\"},{\"Name\":\"NND_abwartend\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"NND abwartend:\",\"ID\":\"NNS_ABWARTEND\"}]},{\"Kategorie\":\"Einsatzart\",\"ID\":\"EINSATZART\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Verkehrsunfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Verkehrsunfall:\",\"ID\":\"VERKEHRSUNFALL\"},{\"Name\":\"Arbeitsunfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Arbeitsunfall:\",\"ID\":\"ARBEITSUNFALL\"},{\"Name\":\"Chirurgischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Chirurgischer Notfall:\",\"ID\":\"CHIRURGISCHER_NOTFALL\"},{\"Name\":\"Gynaekologischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Gynäkologischer Notfall:\",\"ID\":\"GYNAEKOLOGISCHER_NOTFALL\"},{\"Name\":\"Internistischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Internistischer Notfall:\",\"ID\":\"INTERNISTISCHER_NOTFALL\"},{\"Name\":\"Fehleinsatz\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Fehleinsatz:\",\"ID\":\"FEHLEINSATZ\"},{\"Name\":\"Paediatrischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Pädiatrischer Notfall:\",\"ID\":\"PAEDIATRISCHER_NOTFALL\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"}]},{\"Kategorie\":\"Sonstiges\",\"ID\":\"SONSTIGES\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Feuerwehr\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Feuerwehr:\",\"ID\":\"FEUERWEHR\"},{\"Name\":\"Polizei\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Polizei:\",\"ID\":\"POLIZEI\"},{\"Name\":\"Sonstiger_Text_label\",\"Element\":\"label\",\"Label\":\"Sonstige Informationen:\",\"ID\":\"SONSTIGER_TEXT_LABEL\"},{\"Name\":\"Sonstiger_Text\",\"Element\":\"textarea\",\"Rows\":7,\"Cols\":60,\"ID\":\"SONSTIGER_TEXT\"}]}]}", new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(8203), -1L });

            migrationBuilder.InsertData(
                table: "UserLoginAttempts",
                columns: new[] { "Id", "FailedLoginAttempts", "LastLoginAttempt", "userId" },
                values: new object[] { -1L, 0, new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(5371), -1L });

            migrationBuilder.InsertData(
                table: "UserOrganizationRoles",
                columns: new[] { "organizationId", "roleId", "userId", "CreatedDate", "Id", "UpdatedDate" },
                values: new object[] { -1L, -3L, -1L, new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(5259), -1L, new DateTime(2024, 6, 24, 19, 59, 51, 888, DateTimeKind.Utc).AddTicks(5261) });

            migrationBuilder.InsertData(
                table: "TemplateVersions",
                columns: new[] { "Id", "CreatedDate", "TemplateContent", "UpdatedDate", "templateId" },
                values: new object[] { -1L, new DateTime(2024, 6, 24, 19, 59, 51, 889, DateTimeKind.Utc).AddTicks(84), "{\"Name\":\"Protokollschema\",\"Schema\":[{\"Kategorie\":\"Schlüssel\",\"ID\":\"SCHLUESSEL\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Alarmschluessel\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Alarmschlüssel:\",\"ID\":\"ALARMSCHLUESSEL\",\"Mandatory\":1,\"Pattern\":\"^[123][0-9]{3}[NBnb]?$\",\"Placeholder\":\"1000N\"},{\"Name\":\"Auftragsnummer\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"Auftrags Nr:\",\"ID\":\"AUFTRAGSNUMMER\",\"Mandatory\":1,\"DisableHandlerId\":\"Auftragsnummer-Mandatoryhandler\",\"Pattern\":\"^([0-9]+)$\",\"Placeholder\":\"123456\"},{\"Name\":\"Auftragsnummer-Mandatoryhandler\",\"Element\":\"mandatoryhandler\",\"Type\":\"mandatoryhandler\",\"Label\":\"Keine Auftragsnummer:\",\"ID\":\"Auftragsnummer-Mandatoryhandler\"}]},{\"Kategorie\":\"Einsatzort\",\"ID\":\"EINSATZORT-KATEGORIE\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Datum\",\"Element\":\"input\",\"Type\":\"date\",\"Label\":\"Datum:\",\"Mandatory\":1,\"ID\":\"DATUM\"},{\"Name\":\"Ort\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Einsatzort:\",\"Mandatory\":1,\"ID\":\"EINSATZORT\",\"Placeholder\":\"Stuttgart\"},{\"Name\":\"Strasse\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Straße:\",\"Mandatory\":1,\"ID\":\"EINSATSSTRASSE\",\"Placeholder\":\"Hauptstraße\"},{\"Name\":\"PLZ\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"PLZ:\",\"ID\":\"EINSATZPLZ\"},{\"Name\":\"Alarmzeit\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Alarmzeit:\",\"ID\":\"ALARMZEIT\"},{\"Name\":\"Ankunft_HvO\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Ankunft HvO:\",\"ID\":\"ANKUNFT_HVO\"},{\"Name\":\"Einsatzende_HvO\",\"Element\":\"input\",\"Type\":\"time\",\"Mandatory\":1,\"Label\":\"Einsatzende HvO:\",\"ID\":\"EINSATZENDE_HVO\"},{\"Name\":\"RTW_NEF\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RTW/NEF:\",\"ID\":\"RTW_NEF\"},{\"Name\":\"Ankunft_RTW_NEF\",\"Element\":\"input\",\"Type\":\"time\",\"Label\":\"Ankunft RTW/NEF:\",\"ID\":\"ANKUNFT_RTW_NEF\"}]},{\"Kategorie\":\"Fahrzeug\",\"ID\":\"FAHRZEUG\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Einsatzfahrzeug\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Einsatzfahrzeug:\",\"ID\":\"EINSATZFAHRZEUG\",\"Placeholder\":\"x/xx/x\"},{\"Name\":\"RD\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"RD:\",\"ID\":\"RD\",\"Placeholder\":\"RTW\"},{\"Name\":\"Privat_PKW\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Privat PKW:\",\"ID\":\"PRIVAT_PKW\"},{\"Name\":\"Liste_Privat_PKW\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Weitere private PKW:\",\"Placeholder\":\"x-xx-xxxx\",\"ID\":\"LISTE_PRIVAT_PKW\"}]},{\"Kategorie\":\"Einsatzhelfer\",\"ID\":\"EINSATZHELFER\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Helfer\",\"Element\":\"dropdownHelper\",\"Type\":\"dropdown\",\"Options\":\"/Data/DropdownHelfer.json\",\"Label\":\"Helfer:\",\"HelperCollection\":[\"HELFERNAMENDD1\"],\"HelperNames\":[\"\"],\"ID\":\"HELFERNAMENDD\",\"Location\":\"beim Patient\"}]},{\"Kategorie\":\"Atemwege\",\"ID\":\"ATEMWEGE\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Frei\",\"Element\":\"input\",\"Type\":\"radio\",\"RadioGroup\":\"atemwege\",\"Label\":\"frei:\",\"ID\":\"FREI\"},{\"Name\":\"Verlegt\",\"Element\":\"input\",\"Type\":\"radio\",\"RadioGroup\":\"atemwege\",\"Label\":\"verlegt:\",\"ID\":\"VERLEGT\"},{\"Name\":\"Atemweg_Zusatz\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Sonstiges:\",\"ID\":\"ATEMWEGZUSATZ\",\"Placeholder\":\"z.B. verlegtdurch ...\"}]},{\"Kategorie\":\"Belüftung\",\"ID\":\"BELUEFTUNG\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"unauffaellig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unauffällig:\",\"ID\":\"UNAUFFAELLIG\"},{\"Name\":\"Zyanose\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Zyanose:\",\"ID\":\"ZYANOSE\"},{\"Name\":\"Rasseln\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Rasseln:\",\"ID\":\"RASSELN\"},{\"Name\":\"Schnappatmung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Schnappatmung:\",\"ID\":\"SCHNAPPATMUNG\"},{\"Name\":\"Atemnot\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemnot:\",\"ID\":\"ATEMNOT\"},{\"Name\":\"Hyperventilation\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Hyperventilation:\",\"ID\":\"HYPERVENTILATION\"},{\"Name\":\"Atemstillstand\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemstillstand:\",\"ID\":\"ATEMSTILLSTAND\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"}]},{\"Kategorie\":\"Circulation\",\"ID\":\"CIRCULATION\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Puls_Label\",\"Element\":\"label\",\"Label\":\"Puls:\",\"ID\":\"PULS_LABEL\"},{\"Name\":\"regelmaeßig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"regelmäßig:\",\"ID\":\"REGELMAESSIG\"},{\"Name\":\"unregelmaeßig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unregelmäßig:\",\"ID\":\"UNREGELMAESSIG\"},{\"Name\":\"gut_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"gut tastbar:\",\"ID\":\"GUT_TASTBAR\"},{\"Name\":\"schlecht_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"schlecht tastbar:\",\"ID\":\"SCHLECHT_TASTBAR\"},{\"Name\":\"nicht_tastbar\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"nicht tastbar:\",\"ID\":\"NICHT_TASTBAR\"},{\"Name\":\"Haut_Label\",\"Element\":\"label\",\"Label\":\"Haut:\",\"ID\":\"HAUT_LABEL\"},{\"Name\":\"rosig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"rosig:\",\"ID\":\"ROSIG\"},{\"Name\":\"blass\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"blass:\",\"ID\":\"BLASS\"},{\"Name\":\"blau\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"blau:\",\"ID\":\"BLAU\"},{\"Name\":\"rot\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"rot:\",\"ID\":\"ROT\"},{\"Name\":\"warm\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"warm:\",\"ID\":\"WARM\"},{\"Name\":\"kalt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"kalt:\",\"ID\":\"KALT\"},{\"Name\":\"CWERTE_Label\",\"Element\":\"label\",\"Label\":\"Werte:\",\"ID\":\"CWERTE_LABEL\"},{\"Name\":\"Puls\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"Puls:\",\"ID\":\"PULS\",\"Mandatory\":1},{\"Name\":\"RR_sys\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RR sys.:\",\"ID\":\"RR_SYS\"},{\"Name\":\"RR_dia\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"RR dia.:\",\"ID\":\"RR_DIA\"},{\"Name\":\"SPO2\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"SPO²:\",\"ID\":\"SPO2\"},{\"Name\":\"keine_Werte\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"keine Werte:\",\"ID\":\"KEINE_WERTE\"}]},{\"Kategorie\":\"Defizit\",\"ID\":\"DEFIZIT\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Bewusstein\",\"Element\":\"label\",\"Label\":\"Bewusstein:\",\"ID\":\"BEWUSSTEIN\"},{\"Name\":\"orientiert\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"orientiert:\",\"ID\":\"ORIENTIERT\"},{\"Name\":\"desorientiert\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"desorientiert:\",\"ID\":\"DESORIENTIERT\"},{\"Name\":\"getrübt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"getrübt:\",\"ID\":\"GETRÜBT\"},{\"Name\":\"bewusstlos\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"bewusstlos:\",\"ID\":\"BEWUSSTLOS\"},{\"Name\":\"BZ\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"BZ:\",\"ID\":\"BZ\"},{\"Name\":\"Pupillen\",\"Element\":\"label\",\"Label\":\"Pupillen:\",\"ID\":\"PUPILLEN\"},{\"Name\":\"links\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"eng\",\"weit\",\"entrundet\"],\"Label\":\"links:\",\"ID\":\"LINKS\"},{\"Name\":\"rechts\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"eng\",\"weit\",\"entrundet\"],\"Label\":\"rechts:\",\"ID\":\"RECHTS\"},{\"Name\":\"keine_Lichtreflexe\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"links\",\"rechts\",\"beidseitig\"],\"Label\":\"keine Lichtreflexe:\",\"ID\":\"KEINE_LICHTREFLEXE\"},{\"Name\":\"Schmerzen\",\"Element\":\"label\",\"Label\":\"Schmerzen:\",\"ID\":\"SCHMERZEN\"},{\"Name\":\"keine\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"keine:\",\"ID\":\"KEINE\"},{\"Name\":\"leicht\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"leicht:\",\"ID\":\"LEICHT\"},{\"Name\":\"mittel\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"mittel:\",\"ID\":\"MITTEL\"},{\"Name\":\"stark\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"stark:\",\"ID\":\"STARK\"},{\"Name\":\"kolikartig\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"kolikartig:\",\"ID\":\"KOLIKARTIG\"},{\"Name\":\"Schmerz_index\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"],\"Label\":\"Index 1 bis 10:\",\"ID\":\"SCHMERZ_INDEX\"}]},{\"Kategorie\":\"Verletzungen\",\"ID\":\"VERLETZUNGEN\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"unverletzt\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"unverletzt:\",\"ID\":\"UNVERLETZT\"},{\"Name\":\"Schädel_Hirn\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Schädel Hirn:\",\"ID\":\"SCHÄDEL_HIRN\"},{\"Name\":\"Gesicht\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Gesicht:\",\"ID\":\"GESICHT\"},{\"Name\":\"HWS\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"HWS:\",\"ID\":\"HWS\"},{\"Name\":\"Thorax\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Thorax:\",\"ID\":\"THORAX\"},{\"Name\":\"Abdomen\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Abdomen:\",\"ID\":\"ABDOMEN\"},{\"Name\":\"BWS_LWS\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"BWS/LWS:\",\"ID\":\"BWS_LWS\"},{\"Name\":\"Becken\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Becken:\",\"ID\":\"BECKEN\"},{\"Name\":\"Obere_Extremität\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Obere Extremität:\",\"ID\":\"OBERE_EXTREMITÄT\"},{\"Name\":\"Untere_Extremität\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Untere Extremität:\",\"ID\":\"UNTERE_EXTREMITÄT\"},{\"Name\":\"Weichteile\",\"Element\":\"dropdown\",\"Type\":\"dropdown\",\"Options\":[\"-\",\"offen\",\"geschlossen\",\"leicht\",\"mittel\",\"schwer\"],\"Label\":\"Weichteile:\",\"ID\":\"WEICHTEILE\"}]},{\"Kategorie\":\"Maßnahmen\",\"ID\":\"MASSNAHMEN\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Atemwege_freimachen\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Atemwege freimachen:\",\"ID\":\"ATEMWEGE_FREIMACHEN\"},{\"Name\":\"Larynxtubus\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Larynxtubus:\",\"ID\":\"LARYNXTUBUS\"},{\"Name\":\"O2_Gabe\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"O2 Gabe:\",\"ID\":\"O2_GABE\"},{\"Name\":\"l_min\",\"Element\":\"input\",\"Type\":\"number\",\"Label\":\"l/min:\",\"ID\":\"L_MIN\",\"Pattern\":\"\\b([1-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|250)\\b\",\"Placeholder\":\"5\"},{\"Name\":\"Brille_Maske_Beutel\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Brille/Maske/Beutel:\",\"ID\":\"BRILLE_MASKE_BEUTEL\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"},{\"Name\":\"Herzdruckmassage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Herzdruckmassage:\",\"ID\":\"HERZDRUCKMASSAGE\"},{\"Name\":\"AED\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"AED:\",\"ID\":\"AED\"},{\"Name\":\"Defibrilliert\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Defibrilliert:\",\"ID\":\"DEFIBRILLIERT\"},{\"Name\":\"Wundversorgung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Wundversorgung:\",\"ID\":\"WUNDVERSORGUNG\"},{\"Name\":\"HWS_Fixierung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"HWS Fixierung:\",\"ID\":\"HWS_FIXIERUNG\"},{\"Name\":\"NA_Nachforderung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"NA Nachforderung:\",\"ID\":\"NA_NACHFORDERUNG\"},{\"Name\":\"Seitenlage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Seitenlage:\",\"ID\":\"SEITENLAGE\"},{\"Name\":\"Oberkoerper_hoch_sitzend\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Oberkörper hoch/sitzend:\",\"ID\":\"OBERKOERPER_HOCH_SITZEND\"},{\"Name\":\"Flachlagerung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Flachlagerung:\",\"ID\":\"FLACHLAGERUNG\"},{\"Name\":\"Schocklage\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Schocklage:\",\"ID\":\"SCHOCKLAGE\"},{\"Name\":\"Ruhigstellung_mit\",\"Element\":\"input\",\"Type\":\"text\",\"Label\":\"Ruhigstellung mit:\",\"ID\":\"RUHIGSTELLUNG_MIT\",\"Pattern\":\"^[a-zA-Z]+$\",\"Placeholder\":\"z.B. Schiene\"},{\"Name\":\"Absicherung\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Absicherung:\",\"ID\":\"ABSICHERUNG\"},{\"Name\":\"Einweisung_RD\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Einweisung RD:\",\"ID\":\"EINWEISUNG_RD\"},{\"Name\":\"Unterstuetzung_RD\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Unterstützung RD:\",\"ID\":\"UNTERSTUETZUNG_RD\"},{\"Name\":\"NND_abwartend\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"NND abwartend:\",\"ID\":\"NNS_ABWARTEND\"}]},{\"Kategorie\":\"Einsatzart\",\"ID\":\"EINSATZART\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Verkehrsunfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Verkehrsunfall:\",\"ID\":\"VERKEHRSUNFALL\"},{\"Name\":\"Arbeitsunfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Arbeitsunfall:\",\"ID\":\"ARBEITSUNFALL\"},{\"Name\":\"Chirurgischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Chirurgischer Notfall:\",\"ID\":\"CHIRURGISCHER_NOTFALL\"},{\"Name\":\"Gynaekologischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Gynäkologischer Notfall:\",\"ID\":\"GYNAEKOLOGISCHER_NOTFALL\"},{\"Name\":\"Internistischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Internistischer Notfall:\",\"ID\":\"INTERNISTISCHER_NOTFALL\"},{\"Name\":\"Fehleinsatz\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Fehleinsatz:\",\"ID\":\"FEHLEINSATZ\"},{\"Name\":\"Paediatrischer_Notfall\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Pädiatrischer Notfall:\",\"ID\":\"PAEDIATRISCHER_NOTFALL\"},{\"Name\":\"Sonstiges\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Sonstiges:\",\"ID\":\"SONSTIGES\"}]},{\"Kategorie\":\"Sonstiges\",\"ID\":\"SONSTIGES\",\"MARKING\":false,\"Inputs\":[{\"Name\":\"Feuerwehr\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Feuerwehr:\",\"ID\":\"FEUERWEHR\"},{\"Name\":\"Polizei\",\"Element\":\"input\",\"Type\":\"checkbox\",\"Label\":\"Polizei:\",\"ID\":\"POLIZEI\"},{\"Name\":\"Sonstiger_Text_label\",\"Element\":\"label\",\"Label\":\"Sonstige Informationen:\",\"ID\":\"SONSTIGER_TEXT_LABEL\"},{\"Name\":\"Sonstiger_Text\",\"Element\":\"textarea\",\"Rows\":7,\"Cols\":60,\"ID\":\"SONSTIGER_TEXT\"}]}]}", new DateTime(2024, 6, 24, 19, 59, 51, 889, DateTimeKind.Utc).AddTicks(85), -1L });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalUsers_protocolId",
                table: "AdditionalUsers",
                column: "protocolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolContents_protocolId",
                table: "ProtocolContents",
                column: "protocolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolPdfFiles_protocolId",
                table: "ProtocolPdfFiles",
                column: "protocolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_OrganizationId",
                table: "Protocols",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_TemplateId",
                table: "Protocols",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_UserId",
                table: "Protocols",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOrganizations_templateId",
                table: "TemplateOrganizations",
                column: "templateId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_organizationId",
                table: "Templates",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_templateId",
                table: "TemplateVersions",
                column: "templateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempts_userId",
                table: "UserLoginAttempts",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_userId",
                table: "UserMessages",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationRoles_organizationId",
                table: "UserOrganizationRoles",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationRoles_roleId",
                table: "UserOrganizationRoles",
                column: "roleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalUsers");

            migrationBuilder.DropTable(
                name: "ProtocolContents");

            migrationBuilder.DropTable(
                name: "ProtocolPdfFiles");

            migrationBuilder.DropTable(
                name: "TemplateOrganizations");

            migrationBuilder.DropTable(
                name: "TemplateVersions");

            migrationBuilder.DropTable(
                name: "UserLoginAttempts");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropTable(
                name: "UserOrganizationRoles");

            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
