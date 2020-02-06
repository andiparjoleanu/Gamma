using Microsoft.EntityFrameworkCore.Migrations;

namespace GammaAPI.Migrations
{
    public partial class CourseLessonProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lesson",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lesson",
                table: "Courses");
        }
    }
}
