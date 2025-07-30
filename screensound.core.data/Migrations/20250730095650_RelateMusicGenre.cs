using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace screensound.Migrations
{
    /// <inheritdoc />
    public partial class RelateMusicGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenreMusic",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    MusicsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMusic", x => new { x.GenresId, x.MusicsId });
                    table.ForeignKey(
                        name: "FK_GenreMusic_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMusic_Musics_MusicsId",
                        column: x => x.MusicsId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMusic_MusicsId",
                table: "GenreMusic",
                column: "MusicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreMusic");
        }
    }
}
