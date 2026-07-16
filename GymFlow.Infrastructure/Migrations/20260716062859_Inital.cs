using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisterDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "1. Active\n2. Inactive\n3. Suspended\n4. Unsuspended"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DaysPerWeek = table.Column<byte>(type: "tinyint", nullable: false),
                    DurationDays = table.Column<short>(type: "smallint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    AttendanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckIn = table.Column<TimeOnly>(type: "time", nullable: false),
                    CheckOut = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberAttendances_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "Active = 1\nInactive = 2\nExpired = 3\nCancelled = 4\nSuspended = 5\nUnsuspended = 6"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "DeletedAt", "Email", "FullName", "Gender", "IsDeleted", "PhoneNumber", "RegisterDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Khartoum", new DateOnly(1995, 5, 15), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2387), null, "ahmed@gmail.com", "Ahmed Mohamed", 1, false, "01000000001", new DateOnly(2026, 1, 1), "Active", new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2387) },
                    { 2, "Omdurman", new DateOnly(1998, 8, 20), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2398), null, "sara@gmail.com", "Sara Ali", 2, false, "01000000002", new DateOnly(2026, 1, 5), "Active", new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2399) },
                    { 3, "Bahri", new DateOnly(1990, 3, 10), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2405), null, "mohamed@gmail.com", "Mohamed Hassan", 1, false, "01000000003", new DateOnly(2026, 1, 10), "Suspended", new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2406) }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "Id", "CreatedAt", "DaysPerWeek", "DeletedAt", "DurationDays", "IsActive", "IsDeleted", "NameAr", "NameEn", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7731), (byte)7, null, (short)30, true, false, "شهري", "Monthly", 50m, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7734) },
                    { 2, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7743), (byte)7, null, (short)90, true, false, "ربع سنوي", "Quarterly", 130m, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7744) },
                    { 3, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7749), (byte)7, null, (short)365, true, false, "سنوي", "Yearly", 450m, new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7749) }
                });

            migrationBuilder.InsertData(
                table: "MemberSubscriptions",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "EndDate", "IsDeleted", "MemberId", "Price", "StartDate", "Status", "SubscriptionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8660), null, new DateOnly(2026, 1, 31), false, 1, 50m, new DateOnly(2026, 1, 1), "Active", 1, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8664) },
                    { 2, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8682), null, new DateOnly(2026, 4, 5), false, 2, 130m, new DateOnly(2026, 1, 5), "Active", 2, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8683) },
                    { 3, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8688), null, new DateOnly(2026, 1, 10), false, 3, 450m, new DateOnly(2025, 1, 10), "Expired", 3, new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8688) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberAttendances_MemberId",
                table: "MemberAttendances",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_MemberId",
                table: "MemberSubscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_SubscriptionTypeId",
                table: "MemberSubscriptions",
                column: "SubscriptionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberAttendances");

            migrationBuilder.DropTable(
                name: "MemberSubscriptions");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");
        }
    }
}
