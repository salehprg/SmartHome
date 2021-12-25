using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace webAPI.Migrations
{
    public partial class Camera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cameraName = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraModels", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "time",
                value: new DateTime(2021, 9, 7, 11, 51, 44, 281, DateTimeKind.Local).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "time",
                value: new DateTime(2021, 9, 7, 11, 51, 44, 289, DateTimeKind.Local).AddTicks(3738));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "time",
                value: new DateTime(2021, 9, 7, 11, 51, 44, 289, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "SmartplugInfoModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "time",
                value: new DateTime(2021, 9, 7, 11, 51, 44, 289, DateTimeKind.Local).AddTicks(3936));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraModels");

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
        }
    }
}
