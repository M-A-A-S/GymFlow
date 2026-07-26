using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptFooterEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptFooterAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

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
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9993), new DateTime(2026, 7, 26, 7, 32, 13, 245, DateTimeKind.Utc).AddTicks(9994) });

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

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "AddressAr", "AddressEn", "CreatedAt", "Currency", "DeletedAt", "Email", "Facebook", "Instagram", "IsDeleted", "LogoUrl", "NameAr", "NameEn", "Phone", "ReceiptFooterAr", "ReceiptFooterEn", "TaxNumber", "UpdatedAt", "Website" },
                values: new object[] { 1, "الخرطوم، السودان", "Khartoum, Sudan", new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(8625), "SDG", null, "info@gymflow.com", "gymflow", "@gymflow", false, "/images/logo.png", "جيم فلو", "GymFlow Fitness Center", "249912345678", "شكراً لاختياركم جيم فلو", "Thank you for choosing GymFlow", "", new DateTime(2026, 7, 26, 7, 32, 13, 254, DateTimeKind.Utc).AddTicks(8627), "www.gymflow.com" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 520, DateTimeKind.Utc).AddTicks(8493), new DateTime(2026, 7, 24, 7, 51, 39, 520, DateTimeKind.Utc).AddTicks(8497) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 520, DateTimeKind.Utc).AddTicks(8504), new DateTime(2026, 7, 24, 7, 51, 39, 520, DateTimeKind.Utc).AddTicks(8505) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9735), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9739) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9747), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9748) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9751), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9752) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9755), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9760), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9761) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9769), new DateTime(2026, 7, 24, 7, 51, 39, 521, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3302), new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3303) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3313), new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3314) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3319), new DateTime(2026, 7, 24, 7, 51, 39, 523, DateTimeKind.Utc).AddTicks(3320) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8544), new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8547) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8561), new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8562) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8568), new DateTime(2026, 7, 24, 7, 51, 39, 525, DateTimeKind.Utc).AddTicks(8568) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3259), new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3260) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3272), new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3273) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3280), new DateTime(2026, 7, 24, 7, 51, 39, 524, DateTimeKind.Utc).AddTicks(3281) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 527, DateTimeKind.Utc).AddTicks(5760), new DateTime(2026, 7, 24, 7, 51, 39, 527, DateTimeKind.Utc).AddTicks(5763) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 527, DateTimeKind.Utc).AddTicks(5774), new DateTime(2026, 7, 24, 7, 51, 39, 527, DateTimeKind.Utc).AddTicks(5775) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9691), new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9693) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9702), new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9703) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9707), new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9708) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9712), new DateTime(2026, 7, 24, 7, 51, 39, 528, DateTimeKind.Utc).AddTicks(9713) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(58), new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(59) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(71), new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(72) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(78), new DateTime(2026, 7, 24, 7, 51, 39, 531, DateTimeKind.Utc).AddTicks(79) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(246), new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(247) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(260), new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(261) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(267), new DateTime(2026, 7, 24, 7, 51, 39, 532, DateTimeKind.Utc).AddTicks(268) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4983), new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4986) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4999), new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5000) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5005), new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5006) });

            migrationBuilder.UpdateData(
                table: "SalesInvoiceDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5011), new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5012) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(7994), new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(7995) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8010), new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8019), new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(818), new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(819) });

            migrationBuilder.UpdateData(
                table: "SalesPayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(832), new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(833) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6788), new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6792) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6805), new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6806) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6810), new DateTime(2026, 7, 24, 7, 51, 39, 536, DateTimeKind.Utc).AddTicks(6811) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 537, DateTimeKind.Utc).AddTicks(6417), new DateTime(2026, 7, 24, 7, 51, 39, 537, DateTimeKind.Utc).AddTicks(6419) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 537, DateTimeKind.Utc).AddTicks(6427), new DateTime(2026, 7, 24, 7, 51, 39, 537, DateTimeKind.Utc).AddTicks(6428) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5896), new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5897) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5905), new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5906) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5910), new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5911) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5914), new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5915) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5918), new DateTime(2026, 7, 24, 7, 51, 39, 539, DateTimeKind.Utc).AddTicks(5919) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5805), new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5807) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5825), new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5825) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5829), new DateTime(2026, 7, 24, 7, 51, 39, 538, DateTimeKind.Utc).AddTicks(5830) });
        }
    }
}
