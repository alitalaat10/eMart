using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eMart.Migrations
{
    /// <inheritdoc />
    public partial class removeconfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "243c94bf-531a-408b-bd60-d5b209e48a24");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6990046-97a8-4759-a424-6a4f6bae04cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ef6984c-575c-4710-8ebf-bedd1787ca3b", "c9b76ea1-09c2-4345-ba23-9f5f91931c4b", "Admin", "ADMIN" },
                    { "db3790bd-7d28-41cb-b5a8-6e5f7f04908c", "4f4cb0a1-52d9-40d6-8fda-dd3bcdbcd251", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ef6984c-575c-4710-8ebf-bedd1787ca3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db3790bd-7d28-41cb-b5a8-6e5f7f04908c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "243c94bf-531a-408b-bd60-d5b209e48a24", "8fc08571-127a-4a77-908b-b28936bfa4e2", "Admin", "ADMIN" },
                    { "b6990046-97a8-4759-a424-6a4f6bae04cf", "cf458f87-b959-4e16-9d96-a4a5afe0066a", "User", "USER" }
                });
        }
    }
}
