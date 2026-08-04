using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationHoursToTrainerSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DurationHours",
                table: "TrainerSchedules",
                type: "decimal(18,2)",
                nullable: true);

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
                columns: new[] { "CreatedAt", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4761), "Suspended", new DateTime(2026, 8, 4, 13, 3, 27, 98, DateTimeKind.Utc).AddTicks(4762) });

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
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9341), null, new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9342) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9347), null, new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9347) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9348), null, new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9349) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9350), null, new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9350) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DurationHours", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9351), null, new DateTime(2026, 8, 4, 13, 3, 27, 103, DateTimeKind.Utc).AddTicks(9352) });

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

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSchedules_DurationHours",
                table: "TrainerSchedules",
                column: "DurationHours",
                filter: "[DurationHours] IS NOT NULL AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainerSchedules_DurationHours",
                table: "TrainerSchedules");

            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "TrainerSchedules");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 242, DateTimeKind.Utc).AddTicks(6342), new DateTime(2026, 7, 26, 7, 32, 13, 242, DateTimeKind.Utc).AddTicks(6345) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 242, DateTimeKind.Utc).AddTicks(6352), new DateTime(2026, 7, 26, 7, 32, 13, 242, DateTimeKind.Utc).AddTicks(6353) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3613), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3614) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3626), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3626) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3630), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3630) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3633), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3634) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3637), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3637) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3644), new DateTime(2026, 7, 26, 7, 32, 13, 243, DateTimeKind.Utc).AddTicks(3645) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2645), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2646) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2655), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2656) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2661), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(2661) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9971), new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9974) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9988), new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9989) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9993), "Expired", new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9994) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9170), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9171) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9183), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9184) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9191), new DateTime(2026, 7, 26, 7, 32, 13, 244, DateTimeKind.Utc).AddTicks(9191) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 247, DateTimeKind.Utc).AddTicks(6406), new DateTime(2026, 7, 26, 7, 32, 13, 247, DateTimeKind.Utc).AddTicks(6409) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 247, DateTimeKind.Utc).AddTicks(6423), new DateTime(2026, 7, 26, 7, 32, 13, 247, DateTimeKind.Utc).AddTicks(6424) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7595), new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7598) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7607), new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7611), new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7612) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7615), new DateTime(2026, 7, 26, 7, 32, 13, 248, DateTimeKind.Utc).AddTicks(7616) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2886), new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2886) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2897), new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2898) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2902), new DateTime(2026, 7, 26, 7, 32, 13, 250, DateTimeKind.Utc).AddTicks(2903) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(57), new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(58) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(69), new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(69) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(74), new DateTime(2026, 7, 26, 7, 32, 13, 251, DateTimeKind.Utc).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7689), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7692) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7782), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7783) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7787), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7788) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7792), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2849), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2850) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2863), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2864) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2871), new DateTime(2026, 7, 26, 7, 32, 13, 252, DateTimeKind.Utc).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(2262), new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(2262) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(2272), new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(2273) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8008), new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8010) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8018), new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8019) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8022), new DateTime(2026, 7, 26, 7, 32, 13, 253, DateTimeKind.Utc).AddTicks(8023) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(3385), new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(3387) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(3395), new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(3396) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(8625), new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(8627) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3954), new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3956) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3967), new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3968) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3971), new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3972) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3974), new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3975) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3978), new DateTime(2026, 7, 26, 7, 32, 13, 256, DateTimeKind.Utc).AddTicks(3978) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5477), new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5479) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5495), new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5496) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5500), new DateTime(2026, 7, 26, 7, 32, 13, 255, DateTimeKind.Utc).AddTicks(5501) });
        }
    }
}
