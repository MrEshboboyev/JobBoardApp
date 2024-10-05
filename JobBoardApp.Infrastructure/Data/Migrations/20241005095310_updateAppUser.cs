using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAppUser : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AspNetUsers",
                type: "timestamp",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "SuspensionEndDate",
                table: "AspNetUsers",
                type: "timestamp",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "SuspensionReason",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobApplications_JobApplicationId",
                table: "Notifications",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobListings_JobListingId",
                table: "Notifications",
                column: "JobListingId",
                principalTable: "JobListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuspensionEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuspensionReason",
                table: "AspNetUsers");

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
    }
}
