using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seed_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4629d1f6-d91e-4e92-9b65-00cd28a10a06"), "59F0CD01-70B6-4DD5-A6D6-A41B8CA15BCA", "Admin", "ADMIN" },
                    { new Guid("98edbc68-de0a-47dc-a94f-3d3b76e9fc99"), "990C9461-B922-419A-AB90-D6223781A3D0", "Sales", "SALES" },
                    { new Guid("f38700b2-937b-4155-8819-738291e02db5"), "73356855-2611-4F6C-8E11-6D4589D80153", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4629d1f6-d91e-4e92-9b65-00cd28a10a06"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98edbc68-de0a-47dc-a94f-3d3b76e9fc99"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f38700b2-937b-4155-8819-738291e02db5"));
        }
    }
}
