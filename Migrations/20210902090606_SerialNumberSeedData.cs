using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webAPI.Migrations
{
    public partial class SerialNumberSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 9, 2, 13, 36, 5, 970, DateTimeKind.Local).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 9, 2, 13, 36, 5, 974, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 9, 2, 13, 36, 5, 974, DateTimeKind.Local).AddTicks(9215));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 9, 2, 13, 36, 5, 974, DateTimeKind.Local).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 1,
                column: "serialNumber",
                value: "01 123144");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "serialNumber",
                value: "01 663525");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 3,
                column: "serialNumber",
                value: "01 767648");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 4,
                column: "serialNumber",
                value: "01 134214");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 5,
                column: "serialNumber",
                value: "01 534634");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 6,
                column: "serialNumber",
                value: "01 346213");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 7,
                column: "serialNumber",
                value: "01 258754");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 8,
                column: "serialNumber",
                value: "01 245234");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 9,
                column: "serialNumber",
                value: "02 315661");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 10,
                column: "serialNumber",
                value: "03 632647");

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 11,
                column: "serialNumber",
                value: "03 732577");

            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "serialNumber",
                value: "01 01 123698 0000");

            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "Id",
                keyValue: 2,
                column: "serialNumber",
                value: "02 02 767892 0000");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 8, 31, 23, 23, 28, 529, DateTimeKind.Local).AddTicks(2072));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 8, 31, 23, 23, 28, 532, DateTimeKind.Local).AddTicks(4167));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 8, 31, 23, 23, 28, 532, DateTimeKind.Local).AddTicks(4231));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 8, 31, 23, 23, 28, 532, DateTimeKind.Local).AddTicks(4255));

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 1,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 3,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 4,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 5,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 6,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 7,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 8,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 9,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 10,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 11,
                column: "serialNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "serialNumber",
                value: "01-01-123698-0000");

            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "Id",
                keyValue: 2,
                column: "serialNumber",
                value: "02-02-767892-0000");
        }
    }
}
