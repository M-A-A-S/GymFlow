using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeMemberAttendanceCheckoutNullabe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CheckOut",
                table: "MemberAttendances",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CheckOut",
                table: "MemberAttendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8386), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8387) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8395), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8396) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8397), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(8398) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2509), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2509) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2515), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2516) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2518), new DateTime(2026, 7, 16, 9, 31, 23, 926, DateTimeKind.Utc).AddTicks(2518) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1710), new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1710) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1715), new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1715) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1716), new DateTime(2026, 7, 16, 9, 31, 23, 927, DateTimeKind.Utc).AddTicks(1717) });
        }
    }
}
