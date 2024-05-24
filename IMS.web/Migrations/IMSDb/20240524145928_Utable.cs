using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.web.Migrations.IMSDb
{
    /// <inheritdoc />
    public partial class Utable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ProductRateInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ProductRateInfo");
        }
    }
}
