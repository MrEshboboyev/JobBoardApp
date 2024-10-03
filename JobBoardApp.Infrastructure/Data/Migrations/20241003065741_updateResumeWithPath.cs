using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateResumeWithPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resume",
                table: "JobApplications",
                newName: "ResumePath");

            migrationBuilder.AddColumn<string>(
                name: "DefaultResumePath",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultResumePath",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ResumePath",
                table: "JobApplications",
                newName: "Resume");
        }
    }
}
