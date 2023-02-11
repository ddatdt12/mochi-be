using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class RelevantTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelevantTransactionId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RelevantTransactionId",
                table: "Transactions",
                column: "RelevantTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_RelevantTransactionId",
                table: "Transactions",
                column: "RelevantTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transactions_RelevantTransactionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RelevantTransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RelevantTransactionId",
                table: "Transactions");
        }
    }
}
