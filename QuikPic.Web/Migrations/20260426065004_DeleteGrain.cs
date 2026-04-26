using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikPic.Web.Migrations
{
    /// <inheritdoc />
    public partial class DeleteGrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grain",
                table: "Presets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Grain",
                table: "Presets",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
