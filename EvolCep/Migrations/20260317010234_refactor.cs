using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvolCep.Migrations
{
    /// <inheritdoc />
    public partial class refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Memberships_MembershipId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientWorkoutSessions",
                table: "ClientWorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_Clients_MembershipId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "RemainingClasses",
                table: "Memberships",
                newName: "DurationInDays");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkoutSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientWorkoutSessions",
                table: "ClientWorkoutSessions",
                columns: new[] { "ClientId", "WorkoutSessionId" });

            migrationBuilder.CreateTable(
                name: "ClientMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    MembershipPlanId = table.Column<int>(type: "int", nullable: false),
                    RemainingClasses = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientMemberships_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientMemberships_Memberships_MembershipPlanId",
                        column: x => x.MembershipPlanId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientMemberships_ClientId",
                table: "ClientMemberships",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMemberships_MembershipPlanId",
                table: "ClientMemberships",
                column: "MembershipPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientWorkoutSessions",
                table: "ClientWorkoutSessions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "Memberships",
                newName: "RemainingClasses");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Memberships",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Memberships",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientWorkoutSessions",
                table: "ClientWorkoutSessions",
                columns: new[] { "ClientId", "StartDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_MembershipId",
                table: "Clients",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Memberships_MembershipId",
                table: "Clients",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id");
        }
    }
}
