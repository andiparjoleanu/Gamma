using Microsoft.EntityFrameworkCore.Migrations;

namespace GammaAPI.Migrations
{
    public partial class DesiredRoleProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesiredRole",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredRole",
                table: "AspNetUsers");
        }
    }
}
