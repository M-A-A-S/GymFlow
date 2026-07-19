using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerTrainerScheduleGymScheduleEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainerSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerSchedules_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GymSchedules",
                columns: new[] { "Id", "CreatedAt", "Day", "DeletedAt", "EndTime", "Gender", "IsDeleted", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2147), "Sunday", null, new TimeSpan(0, 22, 0, 0, 0), "Male", false, new TimeSpan(0, 16, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2149) },
                    { 2, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2153), "Sunday", null, new TimeSpan(0, 15, 0, 0, 0), "Female", false, new TimeSpan(0, 8, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2153) },
                    { 3, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2154), "Monday", null, new TimeSpan(0, 22, 0, 0, 0), "Male", false, new TimeSpan(0, 16, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2155) },
                    { 4, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2156), "Monday", null, new TimeSpan(0, 15, 0, 0, 0), "Female", false, new TimeSpan(0, 8, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2156) },
                    { 5, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2158), "Tuesday", null, new TimeSpan(0, 22, 0, 0, 0), "Male", false, new TimeSpan(0, 18, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2158) },
                    { 6, new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2160), "Tuesday", null, new TimeSpan(0, 17, 0, 0, 0), "Female", false, new TimeSpan(0, 8, 0, 0, 0), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2161) }
                });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9404), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9406) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9519), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9519) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9521), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(9521) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2105), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2106) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2112), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2112) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2115), new DateTime(2026, 7, 19, 13, 3, 7, 768, DateTimeKind.Utc).AddTicks(2115) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5320), new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5321) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5326), new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5326) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5328), new DateTime(2026, 7, 19, 13, 3, 7, 769, DateTimeKind.Utc).AddTicks(5328) });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "FullName", "HireDate", "IsDeleted", "PhoneNumber", "Salary", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4974), null, "Ahmed Mohamed", new DateOnly(2025, 1, 10), false, "01000000101", 5000m, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4976) },
                    { 2, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4984), null, "Sara Ali", new DateOnly(2025, 3, 15), false, "01000000102", 5500m, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4985) },
                    { 3, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4986), null, "Mohamed Hassan", new DateOnly(2024, 11, 20), false, "01000000103", 6000m, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4986) }
                });

            migrationBuilder.InsertData(
                table: "TrainerSchedules",
                columns: new[] { "Id", "CreatedAt", "Day", "DeletedAt", "EndTime", "IsDeleted", "StartTime", "TrainerId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9577), "Sunday", null, new TimeSpan(0, 12, 0, 0, 0), false, new TimeSpan(0, 8, 0, 0, 0), 1, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9577) },
                    { 2, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9581), "Tuesday", null, new TimeSpan(0, 6, 0, 0, 0), false, new TimeSpan(0, 2, 0, 0, 0), 1, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9581) },
                    { 3, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9583), "Monday", null, new TimeSpan(0, 1, 0, 0, 0), false, new TimeSpan(0, 9, 0, 0, 0), 2, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9583) },
                    { 4, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9584), "Wednesday", null, new TimeSpan(0, 7, 0, 0, 0), false, new TimeSpan(0, 3, 0, 0, 0), 2, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9585) },
                    { 5, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9586), "Thursday", null, new TimeSpan(0, 8, 0, 0, 0), false, new TimeSpan(0, 4, 0, 0, 0), 3, new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9586) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GymSchedules_Day_Gender_StartTime",
                table: "GymSchedules",
                columns: new[] { "Day", "Gender", "StartTime" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSchedules_TrainerId_Day_StartTime",
                table: "TrainerSchedules",
                columns: new[] { "TrainerId", "Day", "StartTime" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GymSchedules");

            migrationBuilder.DropTable(
                name: "TrainerSchedules");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8760), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8761) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8768), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8769) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8770), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(8770) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1354), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1355) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1361), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1361) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1363), new DateTime(2026, 7, 18, 14, 4, 30, 199, DateTimeKind.Utc).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3291), new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3291) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3295), new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3295) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3296), new DateTime(2026, 7, 18, 14, 4, 30, 200, DateTimeKind.Utc).AddTicks(3297) });
        }
    }
}
