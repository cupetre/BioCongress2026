using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icof.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInformationArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Institution",
                table: "TeamMembers",
                type: "character varying(220)",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PeopleGroupId",
                table: "TeamMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "TeamMembers",
                type: "character varying(180)",
                maxLength: 180,
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "PageContents",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "PageContents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "PageContents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HeroBlobName",
                table: "PageContents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "PageContents",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "PageContents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "Icof");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "PageContents",
                type: "character varying(220)",
                maxLength: 220,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "PageContents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EligibilityNotes",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistrationEnabled",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationCtaLabel",
                table: "Events",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Events",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "Draft");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Events",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "Congress");

            migrationBuilder.CreateTable(
                name: "EventAgendaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartsAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndsAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAgendaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAgendaItems_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventPeople",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPeople", x => new { x.EventId, x.TeamMemberId, x.Role });
                    table.ForeignKey(
                        name: "FK_EventPeople_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPeople_TeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LogoBlobName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeopleGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    HeroBlobName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_PeopleGroupId",
                table: "TeamMembers",
                column: "PeopleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_Slug",
                table: "PageContents",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventAgendaItems_EventId",
                table: "EventAgendaItems",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPeople_TeamMemberId",
                table: "EventPeople",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Slug",
                table: "Organizations",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeopleGroups_Slug",
                table: "PeopleGroups",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_PeopleGroups_PeopleGroupId",
                table: "TeamMembers",
                column: "PeopleGroupId",
                principalTable: "PeopleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_PeopleGroups_PeopleGroupId",
                table: "TeamMembers");

            migrationBuilder.DropTable(
                name: "EventAgendaItems");

            migrationBuilder.DropTable(
                name: "EventPeople");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PeopleGroups");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_PeopleGroupId",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_PageContents_Slug",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "Institution",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "PeopleGroupId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "HeroBlobName",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "PageContents");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EligibilityNotes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsRegistrationEnabled",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationCtaLabel",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Events");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "PageContents",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "PageContents",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }
    }
}
