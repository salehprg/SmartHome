using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace webAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    HomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.HomeId);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    slave_id = table.Column<int>(type: "int", nullable: false),
                    serialNumber = table.Column<string>(type: "text", nullable: true),
                    moduleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    running = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScenarioName = table.Column<string>(type: "text", nullable: true),
                    cronjob = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.ScenarioId);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    area = table.Column<string>(type: "text", nullable: true),
                    border = table.Column<string>(type: "text", nullable: true),
                    roomName = table.Column<string>(type: "text", nullable: true),
                    X = table.Column<float>(type: "float", nullable: false),
                    Y = table.Column<float>(type: "float", nullable: false),
                    HomeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_rooms_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "HomeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrokeAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    stroke = table.Column<string>(type: "text", nullable: true),
                    HomeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrokeAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrokeAreas_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "HomeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    deviceId = table.Column<int>(type: "int", nullable: false),
                    delay = table.Column<double>(type: "double", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioActions_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    moduleParentId = table.Column<int>(type: "int", nullable: false),
                    registerid = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    deviceType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devices_modules_moduleParentId",
                        column: x => x.moduleParentId,
                        principalTable: "modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_devices_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartplugInfoModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    time = table.Column<DateTime>(type: "datetime", nullable: false),
                    voltage = table.Column<float>(type: "float", nullable: false),
                    current = table.Column<float>(type: "float", nullable: false),
                    watt = table.Column<float>(type: "float", nullable: false),
                    baseDeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartplugInfoModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartplugInfoModels_devices_baseDeviceId",
                        column: x => x.baseDeviceId,
                        principalTable: "devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Homes",
                column: "HomeId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "ScenarioId", "ScenarioName", "cronjob", "running" },
                values: new object[] { 1, "FlipFlop", "0/15 * * ? * * *", false });

            migrationBuilder.InsertData(
                table: "modules",
                columns: new[] { "Id", "moduleType", "serialNumber", "slave_id" },
                values: new object[,]
                {
                    { 1, 0, "01-01-123698-0000", 1 },
                    { 2, 1, "02-02-767892-0000", 2 }
                });

            migrationBuilder.InsertData(
                table: "ScenarioActions",
                columns: new[] { "Id", "ScenarioId", "delay", "deviceId", "status" },
                values: new object[,]
                {
                    { 1, 1, 5.0, 2, "true" },
                    { 2, 1, 10.0, 4, "true" },
                    { 3, 1, 5.0, 2, "false" },
                    { 4, 1, 10.0, 4, "false" }
                });

            migrationBuilder.InsertData(
                table: "StrokeAreas",
                columns: new[] { "Id", "HomeId", "stroke" },
                values: new object[,]
                {
                    { 1, 1, "M562.71,225V354h-290V319H418.37a6.09,6.09,0,0,0,6.09-6.09V225Z" },
                    { 2, 1, "M8.09,130V347.91A6.09,6.09,0,0,0,14.18,354h54V130Z" },
                    { 3, 1, "M216.37,49.82H358.8V92.5H216.37Z" }
                });

            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "RoomId", "HomeId", "X", "Y", "area", "border", "roomName" },
                values: new object[,]
                {
                    { 2, 1, 142f, 240.8f, "M68.18,130V359.9A6.09,6.09,0,0,0,74.27,366h136a6.09,6.09,0,0,0,6.09-6.09V160H186.21V130Z", "M96,130H68.18V359.9A6.09,6.09,0,0,0,74.27,366h136a6.09,6.09,0,0,0,6.09-6.09V225 M152.71,130H186.21V160H218.5", "آشپزخانه" },
                    { 3, 1, 106f, 66f, "M152.71,130h63.66V8.09A6.09,6.09,0,0,0,210.27,2H8.09A6.09,6.09,0,0,0,2,8.09V123.95A6.09,6.09,0,0,0,8.09,130H96Z", "M152.71,130h63.66V8.09A6.09,6.09,0,0,0,210.27,2H8.09A6.09,6.09,0,0,0,2,8.09V123.95A6.09,6.09,0,0,0,8.09,130H96", "اتاق خواب" },
                    { 4, 1, 468f, 134f, "M358.8,160V49.82a6.09,6.09,0,0,1,6.09-6.09H570.78a6.09,6.09,0,0,1,6.09,6.09V218.9a6.09,6.09,0,0,1-6.09,6.09h-212Z", "M358.8,160V49.82a6.09,6.09,0,0,1,6.09-6.09H570.78a6.09,6.09,0,0,1,6.09,6.09V218.9a6.09,6.09,0,0,1-6.09,6.09h-212", "پذیرایی" },
                    { 5, 1, 320f, 273f, "M216.37,354V92.5H358.8V225H424.39V319H272.71V354Z", "M216.37,225V356 M216.21,162V92.5H358.8V160 M358.8,225H424.39V312.91a6.09,6.09,0,0,1,-6.09,6.09H272.71V356", "هال" }
                });

            migrationBuilder.InsertData(
                table: "devices",
                columns: new[] { "Id", "RoomId", "deviceType", "moduleParentId", "name", "registerid", "status" },
                values: new object[,]
                {
                    { 1, 2, 0, 1, "لامپ کم مصرف", 1, "false" },
                    { 2, 2, 0, 1, "مهتابی", 2, "false" },
                    { 3, 2, 0, 1, "2مهتابی", 3, "false" },
                    { 4, 2, 0, 1, "3مهتابی", 4, "false" },
                    { 5, 2, 0, 1, "4مهتابی", 5, "false" },
                    { 6, 2, 0, 1, "5مهتابی", 6, "false" },
                    { 7, 2, 0, 1, "6مهتابی", 7, "false" },
                    { 8, 2, 0, 1, "7مهتابی", 8, "false" },
                    { 9, 2, 1, 1, "پرده", 9, "50" },
                    { 10, 2, 2, 1, "پنجره", 10, "true" },
                    { 11, 2, 2, 1, "در", 11, "false" },
                    { 12, 3, 3, 2, "کامپیوتر", 1, "true" },
                    { 13, 4, 3, 2, "تلویزیون", 2, "true" }
                });

            migrationBuilder.InsertData(
                table: "SmartplugInfoModels",
                columns: new[] { "Id", "baseDeviceId", "current", "time", "voltage", "watt" },
                values: new object[,]
                {
                    { 1, 12, 5f, new DateTime(2021, 8, 31, 15, 7, 34, 817, DateTimeKind.Local).AddTicks(8231), 209f, 1045f },
                    { 2, 12, 6.3f, new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(901), 208.8f, 1315.44f },
                    { 3, 13, 4.4f, new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(971), 208.5f, 917.4f },
                    { 4, 13, 5.7f, new DateTime(2021, 8, 31, 15, 7, 34, 824, DateTimeKind.Local).AddTicks(996), 210.2f, 1198.14f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_devices_moduleParentId",
                table: "devices",
                column: "moduleParentId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_RoomId",
                table: "devices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_HomeId",
                table: "rooms",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioActions_ScenarioId",
                table: "ScenarioActions",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartplugInfoModels_baseDeviceId",
                table: "SmartplugInfoModels",
                column: "baseDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_StrokeAreas_HomeId",
                table: "StrokeAreas",
                column: "HomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScenarioActions");

            migrationBuilder.DropTable(
                name: "SmartplugInfoModels");

            migrationBuilder.DropTable(
                name: "StrokeAreas");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "Homes");
        }
    }
}
