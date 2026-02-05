using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace booker.api.Migrations.BookerIdentityDb
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93cf9253-0248-423b-8f99-b140e49383b2", "93cf9253-0248-423b-8f99-b140e49383b2", "Admin", "ADMIN" },
                    { "c6347742-7b4a-4417-b440-345ac8232796", "c6347742-7b4a-4417-b440-345ac8232796", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93cf9253-0248-423b-8f99-b140e49383b2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6347742-7b4a-4417-b440-345ac8232796");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b74ddd14-6340-4840-95c2-db12554843e5", 0, "5256af18-a8f6-4787-9966-d06c8f80f2ae", "admin@example.com", true, false, null, "", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEJr9+8D7XFpU1uNInD8v6p8P4H3/5H2uG5jR6T7y+zKqV7F9w==", null, false, "679079A4-9E40-4A9A-A3D8-6A579B0D8A5C", false, "admin" });
        }
    }
}
