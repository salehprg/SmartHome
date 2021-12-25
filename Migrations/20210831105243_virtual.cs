using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webAPI.Migrations
{
    public partial class @virtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioActions_deviceId",
                table: "ScenarioActions",
                column: "deviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioActions_devices_deviceId",
                table: "ScenarioActions",
                column: "deviceId",
                principalTable: "devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioActions_devices_deviceId",
                table: "ScenarioActions");

            migrationBuilder.DropIndex(
                name: "IX_ScenarioActions_deviceId",
                table: "ScenarioActions");

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 7, 34, 817, DateTimeKind.Local).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(971));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(996));
        }
    }
}
