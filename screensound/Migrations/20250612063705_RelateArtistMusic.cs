using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace screensound.Migrations
{
    /// <inheritdoc />
    public partial class RelateArtistMusic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "Musics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musics_ArtistId",
                table: "Musics",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Artists_ArtistId",
                table: "Musics",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Artists_ArtistId",
                table: "Musics");

            migrationBuilder.DropIndex(
                name: "IX_Musics_ArtistId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Musics");
        }
    }
}
