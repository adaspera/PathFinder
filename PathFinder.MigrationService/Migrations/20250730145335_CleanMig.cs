using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.MigrationService.Migrations
{
    /// <inheritdoc />
    public partial class CleanMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FeedId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Timezone = table.Column<string>(type: "text", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => new { x.Id, x.FeedId });
                });

            migrationBuilder.CreateTable(
                name: "FeedInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FeedPublisherName = table.Column<string>(type: "text", nullable: false),
                    FeedPublisherUrl = table.Column<string>(type: "text", nullable: false),
                    FeedLanguage = table.Column<string>(type: "text", nullable: false),
                    FeedStartDate = table.Column<string>(type: "text", nullable: true),
                    FeedEndDate = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FeedId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    ZoneId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => new { x.Id, x.FeedId });
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FeedId = table.Column<string>(type: "text", nullable: false),
                    AgencyId = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    LongName = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => new { x.Id, x.FeedId });
                    table.ForeignKey(
                        name: "FK_Routes_Agencies_FeedId_AgencyId",
                        columns: x => new { x.FeedId, x.AgencyId },
                        principalTable: "Agencies",
                        principalColumns: new[] { "Id", "FeedId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FeedId_AgencyId",
                table: "Routes",
                columns: new[] { "FeedId", "AgencyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedInfos");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}
