using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[]
                {
            Guid.NewGuid().ToString(),
            "User",
            "USER",
            Guid.NewGuid().ToString()  // new concurrency stamp
                });

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[]
               {
           Guid.NewGuid().ToString(),
           "Admin",
           "ADMIN",
           Guid.NewGuid().ToString()
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "AspNetRoles", keyColumn: "Name", keyValues: new object[] { "User", "Admin" });
        }

    }
}
