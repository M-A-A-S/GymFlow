using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeMemberAddressAndBirthDateNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "RegisterDate",
                table: "Members",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Members",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Members",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "RegisterDate",
                table: "Members",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Members",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8660), new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8664) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8682), new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8683) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8688), new DateTime(2026, 7, 16, 6, 28, 55, 889, DateTimeKind.Utc).AddTicks(8688) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2387), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2387) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2398), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2399) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2405), new DateTime(2026, 7, 16, 6, 28, 55, 888, DateTimeKind.Utc).AddTicks(2406) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7731), new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7734) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7743), new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7744) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7749), new DateTime(2026, 7, 16, 6, 28, 55, 890, DateTimeKind.Utc).AddTicks(7749) });
        }
    }
}
