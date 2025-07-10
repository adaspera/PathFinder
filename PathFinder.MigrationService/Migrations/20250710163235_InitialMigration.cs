using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.MigrationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    StopId = table.Column<string>(type: "text", nullable: false),
                    FeedId = table.Column<string>(type: "text", nullable: false),
                    StopName = table.Column<string>(type: "text", nullable: false),
                    StopLat = table.Column<double>(type: "double precision", nullable: false),
                    StopLon = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => new { x.StopId, x.FeedId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stops");
        }
    }
}
