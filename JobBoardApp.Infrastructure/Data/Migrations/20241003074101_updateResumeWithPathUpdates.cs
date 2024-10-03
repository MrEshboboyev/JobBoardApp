using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateResumeWithPathUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "UserProfiles",
                newName: "ResumePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResumePath",
                table: "UserProfiles",
                newName: "ProfilePicturePath");
        }
    }
}
