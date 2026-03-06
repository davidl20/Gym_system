using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvolCep.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WorkoutSessions");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ClientWorkoutSessions",
                newName: "StartDateTime");

            migrationBuilder.RenameIndex(
                name: "IX_ClientWorkoutSessions_WorkoutSessionId_Date",
                table: "ClientWorkoutSessions",
                newName: "IX_ClientWorkoutSessions_WorkoutSessionId_StartDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "WorkoutSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "WorkoutSessions");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "ClientWorkoutSessions",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_ClientWorkoutSessions_WorkoutSessionId_StartDateTime",
                table: "ClientWorkoutSessions",
                newName: "IX_ClientWorkoutSessions_WorkoutSessionId_Date");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkoutSessions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WorkoutSessions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
