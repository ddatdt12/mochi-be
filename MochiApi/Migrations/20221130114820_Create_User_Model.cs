using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MochiApi.Migrations
{
    public partial class Create_User_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[,]
                {
                    { new Guid("077f0ae7-b699-40a3-b22e-1f065705b8e3"), "test4@gmail.com", "123123123" },
                    { new Guid("166dc3bd-54bb-4b8f-9de5-b6b5bcee3266"), "test2@gmail.com", "123123123" },
                    { new Guid("994dade2-d09e-4288-8734-840c863fc0ce"), "test3@gmail.com", "123123123" },
                    { new Guid("d0ee9b2a-71cd-4d32-a778-0461ca0f64ff"), "test@gmail.com", "123123123" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
