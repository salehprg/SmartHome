using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webAPI.Migrations
{
    public partial class DeviceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 9, 4, 13, 40, 8, 105, DateTimeKind.Local).AddTicks(2768));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 9, 4, 13, 40, 8, 108, DateTimeKind.Local).AddTicks(4192));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 9, 4, 13, 40, 8, 108, DateTimeKind.Local).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 9, 4, 13, 40, 8, 108, DateTimeKind.Local).AddTicks(4998));

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 1,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 3,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 4,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 5,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 6,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 7,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 8,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 9,
                column: "deviceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 10,
                column: "deviceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 11,
                column: "deviceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 12,
                column: "deviceType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 13,
                column: "deviceType",
                value: 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 3,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 4,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 5,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 6,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 7,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 8,
                column: "deviceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 9,
                column: "deviceType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 10,
                column: "deviceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 11,
                column: "deviceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 12,
                column: "deviceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "devices",
                keyColumn: "Id",
                keyValue: 13,
                column: "deviceType",
                value: 3);
        }
    }
}
