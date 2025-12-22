using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fota.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class dd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HexFileContent",
                table: "BaseMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BaseMessages",
                keyColumn: "Id",
                keyValue: 1,
                column: "HexFileContent",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseMessages",
                keyColumn: "Id",
                keyValue: 2,
                column: "HexFileContent",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "HexFileContent",
                table: "BaseMessages",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BaseMessages",
                keyColumn: "Id",
                keyValue: 1,
                column: "HexFileContent",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseMessages",
                keyColumn: "Id",
                keyValue: 2,
                column: "HexFileContent",
                value: null);
        }
    }
}
