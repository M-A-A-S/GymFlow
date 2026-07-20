using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryProductSupplierEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DescriptionAr", "DescriptionEn", "ImageUrl", "IsActive", "IsDeleted", "NameAr", "NameEn", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8837), null, null, null, null, true, false, "المكملات الغذائية", "Supplements", new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8840) },
                    { 2, new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8844), null, null, null, null, true, false, "الإكسسوارات", "Accessories", new DateTime(2026, 7, 20, 11, 39, 41, 319, DateTimeKind.Utc).AddTicks(8845) }
                });

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

            migrationBuilder.InsertData(
                table: "MemberAttendances",
                columns: new[] { "Id", "AttendanceDate", "CheckIn", "CheckOut", "CreatedAt", "DeletedAt", "IsDeleted", "MemberId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2026, 1, 15), new TimeOnly(8, 0, 0), new TimeOnly(10, 0, 0), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8653), null, false, 1, new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8653) },
                    { 2, new DateOnly(2026, 1, 16), new TimeOnly(7, 30, 0), new TimeOnly(9, 30, 0), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8657), null, false, 1, new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8658) },
                    { 3, new DateOnly(2026, 1, 15), new TimeOnly(17, 0, 0), new TimeOnly(18, 30, 0), new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8660), null, false, 2, new DateTime(2026, 7, 20, 11, 39, 41, 320, DateTimeKind.Utc).AddTicks(8660) }
                });

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

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "CreatedAt", "DeletedAt", "FullName", "IsDeleted", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Khartoum", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8319), null, "Fitness Nutrition Co.", false, "01000001001", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8319) },
                    { 2, "Omdurman", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8323), null, "Elite Sports Supplies", false, "01000001002", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(8323) }
                });

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

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Code", "CreatedAt", "DeletedAt", "DescriptionAr", "DescriptionEn", "ImageUrl", "IsDeleted", "NameAr", "NameEn", "PurchasePrice", "Quantity", "ReorderLevel", "SalePrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "PRD-000001", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1081), null, null, null, null, false, "بروتين", "Whey Protein", 25000m, 30, 5, 35000m, new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1083) },
                    { 2, 2, "PRD-000002", new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1089), null, null, null, null, false, "قفازات", "Gym Gloves", 7000m, 50, 10, 10000m, new DateTime(2026, 7, 20, 11, 39, 41, 323, DateTimeKind.Utc).AddTicks(1089) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MemberAttendances",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2147), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2149) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2153), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2153) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2154), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2155) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2156), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2156) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2158), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2158) });

            migrationBuilder.UpdateData(
                table: "GymSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2160), new DateTime(2026, 7, 19, 13, 3, 7, 767, DateTimeKind.Utc).AddTicks(2161) });

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

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9577), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9577) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9581), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9581) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9583), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9583) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9584), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9585) });

            migrationBuilder.UpdateData(
                table: "TrainerSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9586), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(9586) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4974), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4976) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4984), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4985) });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4986), new DateTime(2026, 7, 19, 13, 3, 7, 773, DateTimeKind.Utc).AddTicks(4986) });
        }
    }
}
