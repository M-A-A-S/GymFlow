using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchasesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "1. Unpaid\n2. Partial\n3. Paid"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseInvoiceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "1. Cash\n2. Bankak\n3. Fawry"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasePayments_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "PurchaseInvoices",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "InvoiceDate", "InvoiceNo", "IsDeleted", "Notes", "PaymentStatus", "SupplierId", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3163), null, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PI-000001", false, null, "Paid", 1, 3500m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3163) },
                    { 2, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3169), null, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "PI-000002", false, null, "Partial", 2, 1800m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3169) },
                    { 3, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3171), null, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "PI-000003", false, null, "Unpaid", 1, 950m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(3171) }
                });

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

            migrationBuilder.InsertData(
                table: "PurchaseDetails",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "ProductId", "PurchaseInvoiceId", "Quantity", "Total", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7058), null, false, 1, 1, 10m, 2000m, 200m, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7058) },
                    { 2, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7062), null, false, 2, 1, 5m, 1500m, 300m, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7062) },
                    { 3, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7064), null, false, 3, 2, 6m, 1800m, 300m, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7064) },
                    { 4, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7066), null, false, 4, 3, 5m, 950m, 190m, new DateTime(2026, 7, 23, 9, 8, 7, 854, DateTimeKind.Utc).AddTicks(7066) }
                });

            migrationBuilder.InsertData(
                table: "PurchasePayments",
                columns: new[] { "Id", "Amount", "CreatedAt", "DeletedAt", "IsDeleted", "Notes", "PaymentDate", "PaymentMethod", "PurchaseInvoiceId", "ReferenceNo", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2000m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6618), null, false, null, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bankak", 1, "TRX-10001", new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6618) },
                    { 2, 1500m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6622), null, false, null, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fawry", 1, "TRX-10002", new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6623) },
                    { 3, 1000m, new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6625), null, false, null, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bankak", 2, "TRX-10003", new DateTime(2026, 7, 23, 9, 8, 7, 855, DateTimeKind.Utc).AddTicks(6625) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_ProductId",
                table: "PurchaseDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PurchaseInvoiceId",
                table: "PurchaseDetails",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_InvoiceNo",
                table: "PurchaseInvoices",
                column: "InvoiceNo",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_SupplierId",
                table: "PurchaseInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_PurchaseInvoiceId",
                table: "PurchasePayments",
                column: "PurchaseInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.DropTable(
                name: "PurchasePayments");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8837), new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8840) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8844), new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8845) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3157), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3158) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3163), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3163) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3164), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3165) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3166), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3166) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3168), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3168) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3173), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(3173) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8653), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8653) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8657), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8658) });

            migrationBuilder.UpdateData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8660), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8519), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8520) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8531), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8531) });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8533), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(8533) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3203), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3204) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3213), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3214) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3216), new DateTime(2026, 7, 20, 11, 39, 41, 321, DateTimeKind.Utc).AddTicks(3216) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1081), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1083) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1089), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1089) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5383), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5383) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5390), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5390) });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5391), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(5392) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8319), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8319) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8323), new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8323) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5070), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5071) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5075), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5075) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5076), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5077) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5078), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5078) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5079), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(5080) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1599), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1600) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1608), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1608) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1610), new DateTime(2026, 7, 20, 11, 39, 41, 324, DateTimeKind.Utc).AddTicks(1610) });
        }
    }
}
