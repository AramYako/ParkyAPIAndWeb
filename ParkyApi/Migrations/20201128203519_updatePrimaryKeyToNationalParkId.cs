using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyApi.Migrations
{
    public partial class updatePrimaryKeyToNationalParkId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NationalParks",
                newName: "NationalParkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NationalParkId",
                table: "NationalParks",
                newName: "Id");
        }
    }
}
