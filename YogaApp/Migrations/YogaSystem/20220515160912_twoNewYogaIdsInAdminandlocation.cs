using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Migrations.YogaSystem
{
    public partial class twoNewYogaIdsInAdminandlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YogaUserId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YogaUserId",
                table: "Instructor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YogaUserId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "YogaUserId",
                table: "Instructor");
        }
    }
}
