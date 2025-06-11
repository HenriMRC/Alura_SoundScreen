using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace screensound.Migrations
{
    /// <inheritdoc />
    public partial class PopulateMusics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder build)
        {
            const string TABLE = "Musics";
            string[] columns = new string[] { "Name", "YearOfRelease" };
;
            build.InsertData(TABLE, columns, new object[] { "Oceano", 1989 });
            build.InsertData(TABLE, columns, new object[] { "Flor de Lis", 1976 });
            build.InsertData(TABLE, columns, new object[] { "Samurai", 1982 });
            build.InsertData(TABLE, columns, new object[] { "Se", 1992 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder builder)
        {
            builder.Sql("DELETE FROM Musicas");
        }
    }
}
