using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webAPI.Migrations
{
    public partial class serialdecoder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "serialNumber",
                table: "devices",
                type: "text",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serialNumber",
                table: "devices");

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 22, 42, 694, DateTimeKind.Local).AddTicks(7371));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 22, 42, 697, DateTimeKind.Local).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 22, 42, 697, DateTimeKind.Local).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 22, 42, 697, DateTimeKind.Local).AddTicks(9754));
        }
    }
}
