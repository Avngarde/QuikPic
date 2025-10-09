using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikPic.Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Presets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Brightness = table.Column<float>(type: "REAL", nullable: false),
                    Contrast = table.Column<float>(type: "REAL", nullable: false),
                    Saturation = table.Column<float>(type: "REAL", nullable: false),
                    Grayscale = table.Column<float>(type: "REAL", nullable: false),
                    Temperature = table.Column<float>(type: "REAL", nullable: false),
                    Tint = table.Column<float>(type: "REAL", nullable: false),
                    Grain = table.Column<float>(type: "REAL", nullable: false),
                    Vignette = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presets");
        }
    }
}
