using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThanhDT_RemoveTableParentStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentStudent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentStudent",
                columns: table => new
                {
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudent", x => new { x.parent_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_ParentStudent.parent_id",
                        column: x => x.parent_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentStudent.student_id",
                        column: x => x.student_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudent_student_id",
                table: "ParentStudent",
                column: "student_id");
        }
    }
}
