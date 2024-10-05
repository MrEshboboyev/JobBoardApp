using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class deletBehaviorUpdateForNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobApplications_JobApplicationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobListings_JobListingId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobApplications_JobApplicationId",
                table: "Notifications",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobListings_JobListingId",
                table: "Notifications",
                column: "JobListingId",
                principalTable: "JobListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobApplications_JobApplicationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobListings_JobListingId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobApplications_JobApplicationId",
                table: "Notifications",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobListings_JobListingId",
                table: "Notifications",
                column: "JobListingId",
                principalTable: "JobListings",
                principalColumn: "Id");
        }
    }
}
