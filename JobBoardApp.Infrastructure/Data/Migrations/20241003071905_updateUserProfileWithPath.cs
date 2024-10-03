using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateUserProfileWithPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultResumePath",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "UserProfiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "DefaultResumePath",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
