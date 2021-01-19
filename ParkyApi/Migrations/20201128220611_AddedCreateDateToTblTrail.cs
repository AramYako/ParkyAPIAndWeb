using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyApi.Migrations
{
    public partial class AddedCreateDateToTblTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trails",
                newName: "TrailId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Trails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Trails");

            migrationBuilder.RenameColumn(
                name: "TrailId",
                table: "Trails",
                newName: "Id");
        }
    }
}
