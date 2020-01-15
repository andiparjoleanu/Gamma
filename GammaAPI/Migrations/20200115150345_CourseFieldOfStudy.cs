using Microsoft.EntityFrameworkCore.Migrations;

namespace GammaAPI.Migrations
{
    public partial class CourseFieldOfStudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudy",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Courses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Courses");
        }
    }
}
