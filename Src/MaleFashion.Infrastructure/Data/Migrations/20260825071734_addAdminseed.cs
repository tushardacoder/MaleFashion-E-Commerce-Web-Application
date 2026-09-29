using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAdminseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000010"), 0, "B9A6C7D8-E9F0-4012-BC34-D56789012345", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@malefashion.com", true, "Admin", "User", true, null, "ADMIN@MALEFASHION.COM", "ADMIN@MALEFASHION.COM", "AQAAAAIAAYagAAAAEPVfuBMniXS4qIDvUEtH6ibwBNS90i1YycUUlNFfOGW8XMKrNijLFlT+I1ZP9wAMHg==", null, false, "A8F5B6C7-D8E9-4F01-AB23-C45678901234", false, "admin@malefashion.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000010") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000010") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));
        }
    }
}
