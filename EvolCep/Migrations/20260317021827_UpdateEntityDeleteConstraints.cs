using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvolCep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityDeleteConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMemberships_Memberships_MembershipPlanId",
                table: "ClientMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMemberships_Memberships_MembershipPlanId",
                table: "ClientMemberships",
                column: "MembershipPlanId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMemberships_Memberships_MembershipPlanId",
                table: "ClientMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMemberships_Memberships_MembershipPlanId",
                table: "ClientMemberships",
                column: "MembershipPlanId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
