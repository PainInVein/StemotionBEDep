using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThanhDT_Rename_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Lesson",
                newName: "lessonName");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Chapter",
                newName: "chapter_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lessonName",
                table: "Lesson",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "chapter_name",
                table: "Chapter",
                newName: "title");
        }
    }
}
