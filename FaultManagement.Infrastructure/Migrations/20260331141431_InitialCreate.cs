using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FaultManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaultNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FaultNotifications",
                columns: new[] { "Id", "CreatedAtUtc", "CreatedByUserId", "Description", "Location", "Priority", "Status", "Title", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7408), 2, "Description for fault notification 1", "Ankara", 1, 1, "Fault #1", null },
                    { 2, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7427), 2, "Description for fault notification 2", "Izmir", 2, 2, "Fault #2", null },
                    { 3, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7430), 2, "Description for fault notification 3", "Bursa", 3, 3, "Fault #3", null },
                    { 4, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7433), 2, "Description for fault notification 4", "Antalya", 1, 4, "Fault #4", null },
                    { 5, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7435), 2, "Description for fault notification 5", "Gaziantep", 2, 5, "Fault #5", null },
                    { 6, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7588), 2, "Description for fault notification 6", "Konya", 3, 1, "Fault #6", null },
                    { 7, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7598), 2, "Description for fault notification 7", "Kayseri", 1, 2, "Fault #7", null },
                    { 8, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7612), 2, "Description for fault notification 8", "Istanbul", 2, 3, "Fault #8", null },
                    { 9, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7615), 2, "Description for fault notification 9", "Ankara", 3, 4, "Fault #9", null },
                    { 10, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7620), 2, "Description for fault notification 10", "Izmir", 1, 5, "Fault #10", null },
                    { 11, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7623), 2, "Description for fault notification 11", "Bursa", 2, 1, "Fault #11", null },
                    { 12, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7625), 2, "Description for fault notification 12", "Antalya", 3, 2, "Fault #12", null },
                    { 13, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7628), 2, "Description for fault notification 13", "Gaziantep", 1, 3, "Fault #13", null },
                    { 14, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7630), 2, "Description for fault notification 14", "Konya", 2, 4, "Fault #14", null },
                    { 15, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(7686), 2, "Description for fault notification 15", "Kayseri", 3, 5, "Fault #15", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAtUtc", "Email", "PasswordHash", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 31, 14, 14, 30, 796, DateTimeKind.Utc).AddTicks(6382), "admin@example.com", "$2a$11$qkdMlWcYwk.Y.oSlnLpbOuDkBdmHo0UClJ67infR.ETB5HONJ47Mu", 1, "admin" },
                    { 2, new DateTime(2026, 3, 31, 14, 14, 30, 925, DateTimeKind.Utc).AddTicks(5526), "user@example.com", "$2a$11$/cXoAT99P0Q3GtEUeIr3IeUf8.Zt1LQE/IyBQoynSqD.1FTgHpNDC", 2, "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaultNotifications_CreatedByUserId",
                table: "FaultNotifications",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FaultNotifications_Priority",
                table: "FaultNotifications",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_FaultNotifications_Status",
                table: "FaultNotifications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaultNotifications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
