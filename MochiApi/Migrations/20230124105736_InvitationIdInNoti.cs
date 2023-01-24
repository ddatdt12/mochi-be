using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class InvitationIdInNoti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_InvitationId",
                table: "Notification",
                column: "InvitationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Invitations_InvitationId",
                table: "Notification",
                column: "InvitationId",
                principalTable: "Invitations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Invitations_InvitationId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_InvitationId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "InvitationId",
                table: "Notification");
        }
    }
}
