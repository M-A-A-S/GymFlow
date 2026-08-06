using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationHoursToGymSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DurationHours",
                table: "GymSchedules",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(423), new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(425) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(428), new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(429) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4238), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4239) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4243), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4244) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4245), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4245) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4247), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4247) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4248), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4248) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4251), null, new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(4251) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8768), new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8768) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8774), new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8774) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8777), new DateTime(2026, 8, 6, 7, 37, 53, 448, DateTimeKind.Utc).AddTicks(8777) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7756), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7756) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7763), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7763) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7765), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2194), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2194) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2200), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2200) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2203), new DateTime(2026, 8, 6, 7, 37, 53, 449, DateTimeKind.Utc).AddTicks(2203) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(4379), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(4380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(4385), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(4385) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9204), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9205) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9211), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9211) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9213), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9213) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9214), new DateTime(2026, 8, 6, 7, 37, 53, 450, DateTimeKind.Utc).AddTicks(9215) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6526), new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6526) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6532), new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6532) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6534), new DateTime(2026, 8, 6, 7, 37, 53, 451, DateTimeKind.Utc).AddTicks(6535) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(97), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(106), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(106) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(108), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(108) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9681), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9682) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9687), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9688) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9689), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9690) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9691), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(9692) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(6997), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(6997) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(7004), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(7005) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(7008), new DateTime(2026, 8, 6, 7, 37, 53, 452, DateTimeKind.Utc).AddTicks(7008) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(2372), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(2372) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(2380), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(2380) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5476), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5476) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5482), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5482) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5483), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(5484) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(8172), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(8173) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(8176), new DateTime(2026, 8, 6, 7, 37, 53, 453, DateTimeKind.Utc).AddTicks(8177) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(804), new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3375), new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3376) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3381), new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3382) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3383), new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3383) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3385), new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3385) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3386), new DateTime(2026, 8, 6, 7, 37, 53, 455, DateTimeKind.Utc).AddTicks(3387) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6066), new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6069) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6252), new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6253) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6255), new DateTime(2026, 8, 6, 7, 37, 53, 454, DateTimeKind.Utc).AddTicks(6255) });

            migrationBuilder.CreateIndex(
                name: "IX_GymSchedules_DurationHours",
                table: "GymSchedules",
                column: "DurationHours",
                filter: "[DurationHours] IS NOT NULL AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GymSchedules_DurationHours",
                table: "GymSchedules");

            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "GymSchedules");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(4664), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(4666) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(4670), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(4670) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9209), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9209) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9213), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9214) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9215), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9215) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9217), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9217) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9218), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9219) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9259), new DateTime(2026, 8, 4, 13, 3, 27, 96, DateTimeKind.Utc).AddTicks(9260) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4894), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4894) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4900), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4900) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4902), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(4902) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4753), new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4754) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4759), new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4760) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4761), new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4762) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8484), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8485) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8490), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8491) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8493), new DateTime(2026, 8, 4, 13, 3, 27, 97, DateTimeKind.Utc).AddTicks(8494) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(1794), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(1795) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(1800), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6701), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6701) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6706), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6707) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6708), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6709) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6710), new DateTime(2026, 8, 4, 13, 3, 27, 99, DateTimeKind.Utc).AddTicks(6710) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4860), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4860) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4869), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4869) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4871), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(4871) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8801), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8802) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8807), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8808) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8810), new DateTime(2026, 8, 4, 13, 3, 27, 100, DateTimeKind.Utc).AddTicks(8810) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9135), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9136) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9140), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9140) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9142), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9143) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9144), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(9145) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6111), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6111) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6122), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6122) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6125), new DateTime(2026, 8, 4, 13, 3, 27, 101, DateTimeKind.Utc).AddTicks(6125) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(1936), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(1936) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(1941), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(1941) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4986), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4987) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4991), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4991) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4993), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(4993) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(7699), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(7699) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(7703), new DateTime(2026, 8, 4, 13, 3, 27, 102, DateTimeKind.Utc).AddTicks(7703) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(622), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(622) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9341), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9342) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9347), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9347) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9348), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9349) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9350), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9350) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9351), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9352) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4369), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4370) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4379), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4379) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4381), new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(4381) });
        }
    }
}
