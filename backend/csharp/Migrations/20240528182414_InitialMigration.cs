﻿using System;
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
                    ReviewComment = table.Column<string>(type: "text", nullable: true),
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
                values: new object[,]
                {
                    { 1L, "Carstennstraße 58", "Berlin", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1509), "Deutsches Rotes Kreuz e.V.", "Bundesorganisation", "12205", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1512), null },
                    { 2L, "Carstennstraße 58", "Berlin", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1514), "Test Tochter 1", "Bundesorganisation", "12205", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1514), null },
                    { 3L, "Carstennstraße 58", "Berlin", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1526), "Test Tochter 2", "Bundesorganisation", "12205", new DateTime(2024, 5, 28, 18, 24, 13, 937, DateTimeKind.Utc).AddTicks(1527), 2L }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Helfer" },
                    { 2L, "Leiter" },
                    { 3L, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "LastPasswordChangeDate", "Password", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 5, 28, 18, 24, 14, 68, DateTimeKind.Utc).AddTicks(3177), "superadmin@drk.de", "Super", "Admin", new DateTime(2024, 5, 28, 18, 24, 14, 68, DateTimeKind.Utc).AddTicks(3194), "$2a$11$Tj3XyxRg2N.ESLz1wH500egcaLyYAbh9Q4m2RVw8s6lNUYtLYDL3W", new DateTime(2024, 5, 28, 18, 24, 14, 68, DateTimeKind.Utc).AddTicks(3193) },
                    { 2L, new DateTime(2024, 5, 28, 18, 24, 14, 200, DateTimeKind.Utc).AddTicks(4273), "testhelfer@drk.de", "Test", "Helfer", new DateTime(2024, 5, 28, 18, 24, 14, 200, DateTimeKind.Utc).AddTicks(4285), "$2a$11$OMH7Y.q40ezfOvlsxpi8O.rb9nvhN3JSlUa.vcKFjj6W4UuDYSFgq", new DateTime(2024, 5, 28, 18, 24, 14, 200, DateTimeKind.Utc).AddTicks(4285) },
                    { 3L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(8783), "testleiter@drk.de", "Test", "Leiter", new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(8793), "$2a$11$dBZKVTcqbiTSuE4CXp5/2.HwSgRlIBh1Nhbffq391cBdElcrekQ1C", new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(8792) }
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "TemplateContent", "UpdatedDate", "organizationId" },
                values: new object[] { 1L, new DateTime(2024, 5, 28, 18, 24, 14, 330, DateTimeKind.Utc).AddTicks(3540), "Standard-Template für alle Organisationen", "Standard-Template", "\"{\\n    \\\"Name\\\": \\\"Protokollschema\\\",\\n    \\\"Schema\\\": [\\n        {\\n            \\\"Kategorie\\\": \\\"Schlüssel\\\",\\n            \\\"ID\\\": \\\"EINSATZORT\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Auftragsnummer\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"Auftrags Nr: \\\",\\n                    \\\"ID\\\": \\\"AUFTRAGSNUMMER\\\",\\n                    \\\"Mandatory\\\": 1\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Alarmschluessel\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Alarmschlüssel: \\\",\\n                    \\\"ID\\\": \\\"ALARMSCHLUESSEL\\\",\\n                    \\\"Mandatory\\\": 1\\n                },\\n                {\\n                    \\\"Name\\\": \\\"TEst\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"Test: \\\",\\n                    \\\"ID\\\": \\\"TEST\\\",\\n                    \\\"Mandatory\\\": 1\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Einsatzort\\\",\\n            \\\"ID\\\": \\\"EINSATZORT-KATEGORIE\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Datum\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"date\\\",\\n                    \\\"Label\\\": \\\"Datum: \\\",\\n                    \\\"ID\\\": \\\"DATUM\\\",\\n                    \\\"Mandatory\\\": 1\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Ort\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Einsatzort: \\\",\\n                    \\\"ID\\\": \\\"EINSATZORT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Strasse\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Straße: \\\",\\n                    \\\"ID\\\": \\\"EINSATSSTRASSE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Hausnummer\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"Hausnummer: \\\",\\n                    \\\"ID\\\": \\\"EINSATZHAUSNR\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"PLZ\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"PLZ: \\\",\\n                    \\\"ID\\\": \\\"EINSATZPLZ\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Alarmzeit\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"time\\\",\\n                    \\\"Label\\\": \\\"Alarmzeit: \\\",\\n                    \\\"ID\\\": \\\"ALARMZEIT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Ankunft_HvO\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"time\\\",\\n                    \\\"Label\\\": \\\"Ankunft HvO: \\\",\\n                    \\\"ID\\\": \\\"ANKUNFT_HVO\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"RTW_NEF\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"RTW/NEF: \\\",\\n                    \\\"ID\\\": \\\"RTW_NEF\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Einsatzende\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"time\\\",\\n                    \\\"Label\\\": \\\"Einsatzende: \\\",\\n                    \\\"ID\\\": \\\"EINSATZENDE\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Fahrzeug\\\",\\n            \\\"ID\\\": \\\"FAHRZEUG\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Einsatzfahrzeug\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Einsatzfahrzeug: \\\",\\n                    \\\"ID\\\": \\\"EINSATZFAHRZEUG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"RD\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"RD: \\\",\\n                    \\\"ID\\\": \\\"RD\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Privat_PKW\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Privat PKW: \\\",\\n                    \\\"ID\\\": \\\"PRIVAT_PKW\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Einsatzhelfer\\\",\\n            \\\"ID\\\": \\\"EINSATZHELFER\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Helfer\\\",\\n                    \\\"Element\\\": \\\"dropdownHelper\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": \\\"/Data/DropdownHelfer.json\\\",\\n                    \\\"Label\\\": \\\"Helfer: \\\",\\n                    \\\"HelperCollection\\\": [\\n                        \\\"HELFERNAMENDD1\\\"\\n                    ],\\n                    \\\"HelperNames\\\": [\\n                        \\\"Mustermann, Max\\\"\\n                    ],\\n                    \\\"ID\\\": \\\"HELFERNAMENDD\\\",\\n                    \\\"Location\\\": \\\"beim Patient\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Atemwege\\\",\\n            \\\"ID\\\": \\\"ATEMWEGE\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Frei\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"radio\\\",\\n                    \\\"RadioGroup\\\": \\\"atemwege\\\",\\n                    \\\"Label\\\": \\\"frei: \\\",\\n                    \\\"ID\\\": \\\"FREI\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Verlegt\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"radio\\\",\\n                    \\\"RadioGroup\\\": \\\"atemwege\\\",\\n                    \\\"Label\\\": \\\"verlegt: \\\",\\n                    \\\"ID\\\": \\\"VERLEGT\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Belüftung\\\",\\n            \\\"ID\\\": \\\"BELUEFTUNG\\\",\\n            \\\"Mandatory\\\": 1,\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"unauffaellig\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"unauffällig: \\\",\\n                    \\\"ID\\\": \\\"UNAUFFAELLIG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Zyanose\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Zyanose: \\\",\\n                    \\\"ID\\\": \\\"ZYANOSE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Rasseln\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Rasseln: \\\",\\n                    \\\"ID\\\": \\\"RASSELN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Schnappatmung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Schnappatmung: \\\",\\n                    \\\"ID\\\": \\\"SCHNAPPATMUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Atemnot\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Atemnot: \\\",\\n                    \\\"ID\\\": \\\"ATEMNOT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Hyperventilation\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Hyperventilation: \\\",\\n                    \\\"ID\\\": \\\"HYPERVENTILATION\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Atemstillstand\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Atemstillstand: \\\",\\n                    \\\"ID\\\": \\\"ATEMSTILLSTAND\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Sonstiges\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Sonstiges: \\\",\\n                    \\\"ID\\\": \\\"SONSTIGES\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Circulation\\\",\\n            \\\"ID\\\": \\\"CIRCULATION\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Puls_Label\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Puls: \\\",\\n                    \\\"ID\\\": \\\"PULS_LABEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"regelmaeßig\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"regelmäßig: \\\",\\n                    \\\"ID\\\": \\\"REGELMAESSIG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"unregelmaeßig\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"unregelmäßig: \\\",\\n                    \\\"ID\\\": \\\"UNREGELMAESSIG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"gut_tastbar\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"gut tastbar: \\\",\\n                    \\\"ID\\\": \\\"GUT_TASTBAR\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"schlecht_tastbar\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"schlecht tastbar: \\\",\\n                    \\\"ID\\\": \\\"SCHLECHT_TASTBAR\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"nicht_tastbar\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"nicht tastbar: \\\",\\n                    \\\"ID\\\": \\\"NICHT_TASTBAR\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Haut_Label\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Haut: \\\",\\n                    \\\"ID\\\": \\\"HAUT_LABEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"rosig\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"rosig: \\\",\\n                    \\\"ID\\\": \\\"ROSIG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"blass\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"blass: \\\",\\n                    \\\"ID\\\": \\\"BLASS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"blau\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"blau: \\\",\\n                    \\\"ID\\\": \\\"BLAU\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"rot\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"rot: \\\",\\n                    \\\"ID\\\": \\\"ROT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"warm \\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"warm: \\\",\\n                    \\\"ID\\\": \\\"WARM\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"kalt\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"kalt: \\\",\\n                    \\\"ID\\\": \\\"KALT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"CWERTE_Label\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Werte: \\\",\\n                    \\\"ID\\\": \\\"CWERTE_LABEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Puls\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"Puls: \\\",\\n                    \\\"ID\\\": \\\"PULS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"RR_sys\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"RR sys.: \\\",\\n                    \\\"ID\\\": \\\"RR_SYS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"RR_dia\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"RR dia.: \\\",\\n                    \\\"ID\\\": \\\"RR_DIA\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"SPO2\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"SPO²: \\\",\\n                    \\\"ID\\\": \\\"SPO2\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"keine_Werte\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"keine Werte: \\\",\\n                    \\\"ID\\\": \\\"KEINE_WERTE\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Defizit\\\",\\n            \\\"ID\\\": \\\"DEFIZIT\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Bewusstein\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Bewusstein: \\\",\\n                    \\\"ID\\\": \\\"BEWUSSTEIN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"orientiert\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"orientiert: \\\",\\n                    \\\"ID\\\": \\\"ORIENTIERT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"desorientiert\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"desorientiert: \\\",\\n                    \\\"ID\\\": \\\"DESORIENTIERT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"getrübt\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"getrübt: \\\",\\n                    \\\"ID\\\": \\\"GETRÜBT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"bewusstlos\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"bewusstlos: \\\",\\n                    \\\"ID\\\": \\\"BEWUSSTLOS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"BZ\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"BZ: \\\",\\n                    \\\"ID\\\": \\\"BZ\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Pupillen\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Pupillen: \\\",\\n                    \\\"ID\\\": \\\"PUPILLEN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"eng\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"links\\\",\\n                        \\\"rechts\\\",\\n                        \\\"beidseitig\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"eng: \\\",\\n                    \\\"ID\\\": \\\"ENG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"weit\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"links\\\",\\n                        \\\"rechts\\\",\\n                        \\\"beidseitig\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"weit: \\\",\\n                    \\\"ID\\\": \\\"WEIT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"keine_Lichtreflexe\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"links\\\",\\n                        \\\"rechts\\\",\\n                        \\\"beidseitig\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"keine Lichtreflexe: \\\",\\n                    \\\"ID\\\": \\\"KEINE_LICHTREFLEXE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"entrundet\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"links\\\",\\n                        \\\"rechts\\\",\\n                        \\\"beidseitig\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"entrundet: \\\",\\n                    \\\"ID\\\": \\\"ENTRUNDET\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Schmerzen\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Schmerzen: \\\",\\n                    \\\"ID\\\": \\\"SCHMERZEN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"keine\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"keine: \\\",\\n                    \\\"ID\\\": \\\"KEINE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"leicht\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"leicht: \\\",\\n                    \\\"ID\\\": \\\"LEICHT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"mittel\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"mittel: \\\",\\n                    \\\"ID\\\": \\\"MITTEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"stark\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"stark: \\\",\\n                    \\\"ID\\\": \\\"STARK\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"kolikartig\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"kolikartig: \\\",\\n                    \\\"ID\\\": \\\"KOLIKARTIG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Schmerz_index\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Index 1 bis 10: \\\",\\n                    \\\"ID\\\": \\\"SCHMERZ_INDEX\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Verletzungen\\\",\\n            \\\"ID\\\": \\\"EVERLETZUNGEN\\\",\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"unverletzt\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"unverletzt: \\\",\\n                    \\\"ID\\\": \\\"UNVERLETZT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Schädel_Hirn\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Schädel Hirn: \\\",\\n                    \\\"ID\\\": \\\"SCHÄDEL_HIRN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Gesicht\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Gesicht: \\\",\\n                    \\\"ID\\\": \\\"GESICHT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"HWS\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"HWS: \\\",\\n                    \\\"ID\\\": \\\"HWS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Thorax\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Thorax: \\\",\\n                    \\\"ID\\\": \\\"THORAX\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Abdomen\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Abdomen: \\\",\\n                    \\\"ID\\\": \\\"ABDOMEN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"BWS_LWS\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"BWS / LWS: \\\",\\n                    \\\"ID\\\": \\\"BWS_LWS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Becken\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Becken: \\\",\\n                    \\\"ID\\\": \\\"BECKEN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Obere_Extremität\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Obere Extremität: \\\",\\n                    \\\"ID\\\": \\\"OBERE_EXTREMITÄT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Untere_Extremität\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Untere Extremität: \\\",\\n                    \\\"ID\\\": \\\"UNTERE_EXTREMITÄT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Weichteile\\\",\\n                    \\\"Element\\\": \\\"dropdown\\\",\\n                    \\\"Type\\\": \\\"dropdown\\\",\\n                    \\\"Options\\\": [\\n                        \\\"-\\\",\\n                        \\\"offen\\\",\\n                        \\\"geschlossen\\\",\\n                        \\\"leicht\\\",\\n                        \\\"mittel\\\",\\n                        \\\"schwer\\\"\\n                    ],\\n                    \\\"Label\\\": \\\"Weichteile: \\\",\\n                    \\\"ID\\\": \\\"WEICHTEILE\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Maßnahmen\\\",\\n            \\\"ID\\\": \\\"MASSNAHMEN\\\",\\n            \\\"Mandatory\\\": 1,\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Atemwege_freimachen\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Atemwege freimachen: \\\",\\n                    \\\"ID\\\": \\\"ATEMWEGE_FREIMACHEN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Larynxtubus\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Larynxtubus: \\\",\\n                    \\\"ID\\\": \\\"LARYNXTUBUS\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"O2_Gabe\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"O2 Gabe: \\\",\\n                    \\\"ID\\\": \\\"O2_GABE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"l_min\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"number\\\",\\n                    \\\"Label\\\": \\\"l/min: \\\",\\n                    \\\"ID\\\": \\\"L_MIN\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Brille_Maske_Beutel\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Brille / Maske / Beutel: \\\",\\n                    \\\"ID\\\": \\\"BRILLE_MASKE_BEUTEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Sonstiges\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Sonstiges: \\\",\\n                    \\\"ID\\\": \\\"SONSTIGES\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Herzdruckmassage\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Herzdruckmassage: \\\",\\n                    \\\"ID\\\": \\\"HERZDRUCKMASSAGE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"AED\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"AED: \\\",\\n                    \\\"ID\\\": \\\"AED\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Defibrilliert\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"text\\\",\\n                    \\\"Label\\\": \\\"Defibrilliert: \\\",\\n                    \\\"ID\\\": \\\"DEFIBRILLIERT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Wundversorgung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Wundversorgung: \\\",\\n                    \\\"ID\\\": \\\"WUNDVERSORGUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"HWS_Fixierung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"HWS Fixierung: \\\",\\n                    \\\"ID\\\": \\\"HWS_FIXIERUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"NA_Nachforderung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"NA Nachforderung: \\\",\\n                    \\\"ID\\\": \\\"NA_NACHFORDERUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Seitenlage\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Seitenlage: \\\",\\n                    \\\"ID\\\": \\\"SEITENLAGE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Oberkoerper_hoch_sitzend\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Oberkörper hoch/sitzend: \\\",\\n                    \\\"ID\\\": \\\"OBERKOERPER_HOCH_SITZEND\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Flachlagerung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Flachlagerung: \\\",\\n                    \\\"ID\\\": \\\"FLACHLAGERUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Schocklage\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Schocklage: \\\",\\n                    \\\"ID\\\": \\\"SCHOCKLAGE\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Ruhigstellung_mit\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Ruhigstellung mit: \\\",\\n                    \\\"ID\\\": \\\"RUHIGSTELLUNG_MIT\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Absicherung\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Absicherung: \\\",\\n                    \\\"ID\\\": \\\"ABSICHERUNG\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Einweisung_RD\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Einweisung RD: \\\",\\n                    \\\"ID\\\": \\\"EINWEISUNG_RD\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Unterstuetzung_RD\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Unterstützung RD: \\\",\\n                    \\\"ID\\\": \\\"UNTERSTUETZUNG_RD\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"NND_abwartend\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"NND abwartend: \\\",\\n                    \\\"ID\\\": \\\"NNS_ABWARTEND\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Einsatzart\\\",\\n            \\\"ID\\\": \\\"EINSATZART\\\",\\n            \\\"Mandatory\\\": 1,\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Verkehrsunfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Verkehrsunfall: \\\",\\n                    \\\"ID\\\": \\\"VERKEHRSUNFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Arbeitsunfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Arbeitsunfall: \\\",\\n                    \\\"ID\\\": \\\"ARBEITSUNFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Chirurgischer_Notfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Chirurgischer Notfall: \\\",\\n                    \\\"ID\\\": \\\"CHIRURGISCHER_NOTFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Gynaekologischer_Notfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Gynäkologischer Notfall: \\\",\\n                    \\\"ID\\\": \\\"GYNAEKOLOGISCHER_NOTFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Internistischer_Notfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Internistischer Notfall: \\\",\\n                    \\\"ID\\\": \\\"INTERNISTISCHER_NOTFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Fehleinsatz\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Fehleinsatz: \\\",\\n                    \\\"ID\\\": \\\"FEHLEINSATZ\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Paediatrischer_Notfall\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Pädiatrischer Notfall: \\\",\\n                    \\\"ID\\\": \\\"PAEDIATRISCHER_NOTFALL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Sonstiges\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Sonstiges: \\\",\\n                    \\\"ID\\\": \\\"SONSTIGES\\\"\\n                }\\n            ]\\n        },\\n        {\\n            \\\"Kategorie\\\": \\\"Sonstiges\\\",\\n            \\\"ID\\\": \\\"SONSTIGES\\\",\\n            \\\"Mandatory\\\": 1,\\n            \\\"Inputs\\\": [\\n                {\\n                    \\\"Name\\\": \\\"Feuerwehr\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Feuerwehr: \\\",\\n                    \\\"ID\\\": \\\"FEUERWEHR\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Polizei\\\",\\n                    \\\"Element\\\": \\\"input\\\",\\n                    \\\"Type\\\": \\\"checkbox\\\",\\n                    \\\"Label\\\": \\\"Polizei: \\\",\\n                    \\\"ID\\\": \\\"POLIZEI\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Sonstiger_Text_label\\\",\\n                    \\\"Element\\\": \\\"label\\\",\\n                    \\\"Label\\\": \\\"Sonstige Informationen:  \\\",\\n                    \\\"ID\\\": \\\"SONSTIGER_TEXT_LABEL\\\"\\n                },\\n                {\\n                    \\\"Name\\\": \\\"Sonstiger_Text\\\",\\n                    \\\"Element\\\": \\\"textarea\\\",\\n                    \\\"Rows\\\": 7,\\n                    \\\"Cols\\\": 60,\\n                    \\\"ID\\\": \\\"SONSTIGER_TEXT\\\"\\n                }\\n            ]\\n        }\\n    ]\\n}\"", new DateTime(2024, 5, 28, 18, 24, 14, 330, DateTimeKind.Utc).AddTicks(3548), 1L });

            migrationBuilder.InsertData(
                table: "UserLoginAttempts",
                columns: new[] { "Id", "FailedLoginAttempts", "LastLoginAttempt", "userId" },
                values: new object[,]
                {
                    { 1L, 0, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9374), 1L },
                    { 2L, 0, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9377), 2L },
                    { 3L, 0, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9378), 3L }
                });

            migrationBuilder.InsertData(
                table: "UserOrganizationRoles",
                columns: new[] { "organizationId", "roleId", "userId", "CreatedDate", "Id", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, 3L, 1L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9329), 1L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9329) },
                    { 3L, 1L, 2L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9331), 2L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9332) },
                    { 2L, 2L, 3L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9333), 3L, new DateTime(2024, 5, 28, 18, 24, 14, 329, DateTimeKind.Utc).AddTicks(9334) }
                });

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
