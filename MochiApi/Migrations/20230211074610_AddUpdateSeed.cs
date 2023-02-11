using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class AddUpdateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: "2");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: "3");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: "4");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: "5");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: "6");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 7,
                column: "Icon",
                value: "7");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 8,
                column: "Icon",
                value: "8");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Type" },
                values: new object[] { "Đi vay", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 16,
                column: "Type",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Trả Nợ", 3, null });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Thu Nợ", 5, null });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "3", "Làm đẹp", null });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Quà tặng và quyên góp", null });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Dịch vụ trực tuyến", null });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Ăn uống");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 1, "2", "Di chuyển" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "3", "Thuê nhà" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 1, "4", "Hóa đơn điện thoại" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "5", "Hóa đơn internet" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 1, "6", "Hóa đơn tiện ích khác", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 2, "7", "Sửa & trang trí khác", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 1, "8", "Bảo dưỡng xe", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Group", "Name" },
                values: new object[] { 2, "Khám sức khỏe" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Group", "Name" },
                values: new object[] { 1, "Thể dục thể thao" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Group", "Name", "Type" },
                values: new object[] { 0, "Lương", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Tiền ăn vặt", 0, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Thu nhập khác", 0, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 4, "Đầu tư", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Đi vay", 2, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Cho vay", 4, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Trả Nợ", 3, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Thu Nợ", 5, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "3", "Làm đẹp", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Quà tặng và quyên góp", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Dịch vụ trực tuyến", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Group", "Name", "Type" },
                values: new object[] { 1, "Ăn uống", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 1, "2", "Di chuyển", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 1, "3", "Thuê nhà", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 1, "4", "Hóa đơn điện thoại" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 1, "5", "Hóa đơn internet" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 1, "6", "Hóa đơn tiện ích khác" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 2, "7", "Sửa & trang trí khác", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "8", "Bảo dưỡng xe", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 2, "Khám sức khỏe", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Thể dục thể thao", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Lương", 0, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Tiền ăn vặt", 0, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Thu nhập khác", 0, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 4, "Đầu tư", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Đi vay", 2, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Cho vay", 4, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Trả Nợ", 3, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Thu Nợ", 5, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Group", "Icon", "Name", "Type", "WalletId" },
                values: new object[] { 3, "3", "Làm đẹp", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Quà tặng và quyên góp", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 3, "4", "Dịch vụ trực tuyến", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Group", "Name" },
                values: new object[] { 1, "Ăn uống" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "2", "Di chuyển", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "3", "Thuê nhà", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "4", "Hóa đơn điện thoại", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "5", "Hóa đơn internet", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "6", "Hóa đơn tiện ích khác", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 2, "7", "Sửa & trang trí khác", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "8", "Bảo dưỡng xe", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 2, "Khám sức khỏe", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 1, "Thể dục thể thao", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Lương", 0, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Tiền ăn vặt", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Thu nhập khác", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 4, "Đầu tư", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Đi vay", 2, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Cho vay", 4, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Trả Nợ", 3, 3 });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Group", "Icon", "Name", "Type", "WalletId" },
                values: new object[,]
                {
                    { 81, 4, "1", "Thu Nợ", 5, 3 },
                    { 82, 3, "3", "Làm đẹp", 1, 3 },
                    { 83, 3, "4", "Quà tặng và quyên góp", 1, 3 },
                    { 84, 3, "4", "Dịch vụ trực tuyến", 1, 3 },
                    { 85, 1, "1", "Ăn uống", 1, 4 },
                    { 86, 1, "2", "Di chuyển", 1, 4 },
                    { 87, 1, "3", "Thuê nhà", 1, 4 },
                    { 88, 1, "4", "Hóa đơn điện thoại", 1, 4 },
                    { 89, 1, "5", "Hóa đơn internet", 1, 4 },
                    { 90, 1, "6", "Hóa đơn tiện ích khác", 1, 4 },
                    { 91, 2, "7", "Sửa & trang trí khác", 1, 4 },
                    { 92, 1, "8", "Bảo dưỡng xe", 1, 4 },
                    { 93, 2, "1", "Khám sức khỏe", 1, 4 },
                    { 94, 1, "1", "Thể dục thể thao", 1, 4 },
                    { 95, 0, "1", "Lương", 0, 4 },
                    { 96, 0, "1", "Tiền ăn vặt", 0, 4 },
                    { 97, 0, "1", "Thu nhập khác", 0, 4 },
                    { 98, 4, "1", "Đầu tư", 1, 4 },
                    { 99, 4, "1", "Đi vay", 2, 4 },
                    { 100, 4, "1", "Cho vay", 4, 4 },
                    { 101, 4, "1", "Trả Nợ", 3, 4 },
                    { 102, 4, "1", "Thu Nợ", 5, 4 },
                    { 103, 3, "3", "Làm đẹp", 1, 4 },
                    { 104, 3, "4", "Quà tặng và quyên góp", 1, 4 },
                    { 105, 3, "4", "Dịch vụ trực tuyến", 1, 4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 7,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 8,
                column: "Icon",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Type" },
                values: new object[] { "Nợ", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 16,
                column: "Type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Ăn uống", 1, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Di chuyển", 1, 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Thuê nhà", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Hóa đơn điện thoại", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Hóa đơn internet", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Hóa đơn tiện ích khác");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 2, "1", "Sửa & trang trí khác" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "1", "Bảo dưỡng xe" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 2, "1", "Khám sức khỏe" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "1", "Thể dục thể thao" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 0, "1", "Lương", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 0, "1", "Tiền ăn vặt", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 0, "1", "Thu nhập khác", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Group", "Name" },
                values: new object[] { 4, "Đầu tư" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Group", "Name" },
                values: new object[] { 4, "Nợ" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Group", "Name", "Type" },
                values: new object[] { 4, "Cho vay", 1 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Ăn uống", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Di chuyển", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 1, "Thuê nhà", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Hóa đơn điện thoại", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Hóa đơn internet", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Hóa đơn tiện ích khác", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 2, "Sửa & trang trí khác", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Bảo dưỡng xe", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 2, "1", "Khám sức khỏe", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Thể dục thể thao", 2 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Group", "Name", "Type" },
                values: new object[] { 0, "Lương", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 0, "1", "Tiền ăn vặt", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "Group", "Icon", "Name", "Type" },
                values: new object[] { 0, "1", "Thu nhập khác", 0 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 4, "1", "Đầu tư" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 4, "1", "Nợ" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Group", "Icon", "Name" },
                values: new object[] { 4, "1", "Cho vay" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Ăn uống", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Di chuyển", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 1, "Thuê nhà", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Hóa đơn điện thoại", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Hóa đơn internet", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Hóa đơn tiện ích khác", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 2, "Sửa & trang trí khác", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 1, "Bảo dưỡng xe", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 2, "Khám sức khỏe", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Thể dục thể thao", 1, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Lương", 0, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Tiền ăn vặt", 0, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Group", "Icon", "Name", "Type", "WalletId" },
                values: new object[] { 0, "1", "Thu nhập khác", 0, 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 4, "1", "Đầu tư", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 4, "1", "Nợ", 3 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Group", "Name" },
                values: new object[] { 4, "Cho vay" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Ăn uống", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Di chuyển", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Thuê nhà", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Hóa đơn điện thoại", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Icon", "Name", "WalletId" },
                values: new object[] { "1", "Hóa đơn internet", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 1, "1", "Hóa đơn tiện ích khác", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "Group", "Icon", "Name", "WalletId" },
                values: new object[] { 2, "1", "Sửa & trang trí khác", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 1, "Bảo dưỡng xe", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "Group", "Name", "WalletId" },
                values: new object[] { 2, "Khám sức khỏe", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 1, "Thể dục thể thao", 1, 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Lương", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "Name", "WalletId" },
                values: new object[] { "Tiền ăn vặt", 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "Group", "Name", "Type", "WalletId" },
                values: new object[] { 0, "Thu nhập khác", 0, 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Đầu tư", 1, 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Nợ", 1, 4 });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "Name", "Type", "WalletId" },
                values: new object[] { "Cho vay", 1, 4 });
        }
    }
}
