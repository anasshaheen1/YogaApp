using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Migrations.YogaSystem
{
    public partial class newcolumnInParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "YogaUserID",
                table: "Participant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
              name: "YogaUserID",
              table: "Participant");
        }
    }
}
