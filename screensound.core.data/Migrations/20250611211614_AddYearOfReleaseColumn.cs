using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace screensound.Migrations
{
    /// <inheritdoc />
    public partial class AddYearOfReleaseColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfRelease",
                table: "Musics",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearOfRelease",
                table: "Musics");
        }
    }
}
