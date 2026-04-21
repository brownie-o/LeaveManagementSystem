using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52f87679-a914-475a-85a5-751ae37def2e", "3", "Administrator", "ADMINISTRATOR" },
                    { "7656f3b9-4def-48a4-a897-8e3cb4f2600a", "2", "Supervisor", "SUPERVISOR" },
                    { "b9814199-7c17-41be-94e1-8fc20fb40230", "1", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2117946d-7479-4511-a6ed-7ac18c5036de", 0, "4", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEFgcZPJY56FWtyEsQlwT2/r/ZOpz5D7u2HnTadem5MSYrFdlEZBGuTeep3NJr35Z+g==", null, false, "5", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "52f87679-a914-475a-85a5-751ae37def2e", "2117946d-7479-4511-a6ed-7ac18c5036de" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7656f3b9-4def-48a4-a897-8e3cb4f2600a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9814199-7c17-41be-94e1-8fc20fb40230");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "52f87679-a914-475a-85a5-751ae37def2e", "2117946d-7479-4511-a6ed-7ac18c5036de" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52f87679-a914-475a-85a5-751ae37def2e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2117946d-7479-4511-a6ed-7ac18c5036de");
        }
    }
}
