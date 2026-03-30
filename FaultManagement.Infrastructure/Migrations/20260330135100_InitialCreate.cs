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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    { new Guid("00001001-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8626), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 1", "Ankara", 1, 1, "Fault #1", null },
                    { new Guid("00001002-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8654), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 2", "Izmir", 2, 2, "Fault #2", null },
                    { new Guid("00001003-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8661), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 3", "Bursa", 3, 3, "Fault #3", null },
                    { new Guid("00001004-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8676), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 4", "Antalya", 1, 4, "Fault #4", null },
                    { new Guid("00001005-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8682), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 5", "Gaziantep", 2, 5, "Fault #5", null },
                    { new Guid("00001006-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8694), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 6", "Konya", 3, 1, "Fault #6", null },
                    { new Guid("00001007-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8803), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 7", "Kayseri", 1, 2, "Fault #7", null },
                    { new Guid("00001008-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8824), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 8", "Istanbul", 2, 3, "Fault #8", null },
                    { new Guid("00001009-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8830), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 9", "Ankara", 3, 4, "Fault #9", null },
                    { new Guid("00001010-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8878), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 10", "Izmir", 1, 5, "Fault #10", null },
                    { new Guid("00001011-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8885), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 11", "Bursa", 2, 1, "Fault #11", null },
                    { new Guid("00001012-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8895), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 12", "Antalya", 3, 2, "Fault #12", null },
                    { new Guid("00001013-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8916), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 13", "Gaziantep", 1, 3, "Fault #13", null },
                    { new Guid("00001014-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8922), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 14", "Konya", 2, 4, "Fault #14", null },
                    { new Guid("00001015-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(8935), new Guid("20000000-0000-0000-0000-000000000001"), "Description for fault notification 15", "Kayseri", 3, 5, "Fault #15", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAtUtc", "Email", "PasswordHash", "Role", "UserName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 505, DateTimeKind.Utc).AddTicks(8113), "admin@example.com", "$2a$11$23sv2SqJqbPL1f.JF3S0NOEn42k1qoZl1ePTXdxT934oi0MDrHzsG", 1, "admin" },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 13, 50, 59, 632, DateTimeKind.Utc).AddTicks(7072), "user@example.com", "$2a$11$sLhoAJr70lMO7IjeKWb6tOqH1n1a.zVUx3kELSt4dyn/2/QYramEi", 2, "user" }
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
