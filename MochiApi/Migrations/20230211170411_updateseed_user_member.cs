using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class updateseed_user_member : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 1, 1 },
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 2, 2 },
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 3, 3 },
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 4, 4 },
                column: "Status",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 1, 1 },
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 2, 2 },
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 3, 3 },
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "WalletMember",
                keyColumns: new[] { "UserId", "WalletId" },
                keyValues: new object[] { 4, 4 },
                column: "Status",
                value: 2);
        }
    }
}
