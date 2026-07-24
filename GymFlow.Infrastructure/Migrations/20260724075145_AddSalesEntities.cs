using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "1. Unpaid\n2. Partial\n3. Paid\n4. Cancelled"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceDetails_SalesInvoices_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesInvoiceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "1. Cash\n2. Bankak\n3. Fawry"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false),
                    VoidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoidReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoidedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPayments_SalesInvoices_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.InsertData(
                table: "SalesInvoices",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Discount", "InvoiceDate", "InvoiceNo", "IsDeleted", "MemberId", "NetAmount", "Notes", "PaidAmount", "RemainingBalance", "Status", "SubTotal", "Tax", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(7994), null, 0m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-0001", false, 1, 150m, "Monthly membership + product", 150m, 0m, "Paid", 150m, 0m, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(7995) },
                    { 2, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8010), null, 20m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-0002", false, 2, 230m, "Gold subscription", 100m, 130m, "Partial", 250m, 0m, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8011) },
                    { 3, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8019), null, 0m, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-0003", false, null, 80m, "Walk-in customer", 0m, 80m, "Unpaid", 80m, 0m, new DateTime(2026, 7, 24, 7, 51, 39, 533, DateTimeKind.Utc).AddTicks(8020) }
                });

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

            migrationBuilder.InsertData(
                table: "SalesInvoiceDetails",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Discount", "IsDeleted", "ItemId", "ItemType", "Quantity", "SalesInvoiceId", "Total", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4983), null, "Monthly Gym Membership", 0m, false, 1, 2, 1m, 1, 100m, 100m, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4986) },
                    { 2, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(4999), null, "Protein Powder", 0m, false, 1, 1, 1m, 1, 50m, 50m, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5000) },
                    { 3, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5005), null, "Gold Membership", 20m, false, 2, 2, 1m, 2, 230m, 250m, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5006) },
                    { 4, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5011), null, "Gym Gloves", 0m, false, 2, 1, 1m, 3, 80m, 80m, new DateTime(2026, 7, 24, 7, 51, 39, 534, DateTimeKind.Utc).AddTicks(5012) }
                });

            migrationBuilder.InsertData(
                table: "SalesPayments",
                columns: new[] { "Id", "Amount", "CreatedAt", "DeletedAt", "IsDeleted", "IsVoided", "Notes", "PaymentDate", "PaymentMethod", "ReferenceNo", "SalesInvoiceId", "UpdatedAt", "VoidDate", "VoidReason", "VoidedBy" },
                values: new object[,]
                {
                    { 1, 150m, new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(818), null, false, false, "Full payment", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", "REC-0001", 1, new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(819), null, null, null },
                    { 2, 100m, new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(832), null, false, false, "First payment", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", "REC-0002", 2, new DateTime(2026, 7, 24, 7, 51, 39, 535, DateTimeKind.Utc).AddTicks(833), null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceDetails_SalesInvoiceId",
                table: "SalesInvoiceDetails",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_InvoiceNo",
                table: "SalesInvoices",
                column: "InvoiceNo",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_MemberId",
                table: "SalesInvoices",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPayments_SalesInvoiceId",
                table: "SalesPayments",
                column: "SalesInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesInvoiceDetails");

            migrationBuilder.DropTable(
                name: "SalesPayments");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(1280), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(1283) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(1288), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(1289) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4386), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4387) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4391), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4392) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4393), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4393) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4394), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4395) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4396), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4396) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4399), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(4399) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8404), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8404) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8409), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8410) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8412), new DateTime(2026, 7, 23, 9, 8, 7, 852, DateTimeKind.Utc).AddTicks(8412) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7047), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7048) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7055), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7055) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7057), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1654), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1655) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1660), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1661) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1663), new DateTime(2026, 7, 23, 9, 8, 7, 853, DateTimeKind.Utc).AddTicks(1663) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(2983), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(2983) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(2988), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(2988) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7058), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7062), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7062) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7064), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7064) });

            migrationBuilder.UpdateData(
                table: "PurchaseDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7066), new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7066) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3163), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3163) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3169), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3169) });

            migrationBuilder.UpdateData(
                table: "PurchaseInvoices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3171), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3171) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6618), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6618) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6622), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6623) });

            migrationBuilder.UpdateData(
                table: "PurchasePayments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6625), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6625) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9445), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9445) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9449), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9449) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9451), new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(9451) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(1824), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(1825) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(1829), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(1829) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8269), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8274), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8274) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8275), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8276) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8277), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8277) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8278), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(8279) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4823), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4824) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4833), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4833) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4834), new DateTime(2026, 7, 23, 9, 8, 7, 856, DateTimeKind.Utc).AddTicks(4835) });
        }
    }
}
