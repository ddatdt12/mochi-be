using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class UnknownParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnknownParticipantsStr",
                table: "Transactions",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnknownParticipantsStr",
                table: "Transactions");
        }
    }
}
