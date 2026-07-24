using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Services;
using GymFlow.Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services
{
    public class SalesInvoiceServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly Mock<IInventoryService> _inventoryMock;
        private readonly SalesInvoiceService _service;

        #endregion


        #region ========================= Constructors =========================
        public SalesInvoiceServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<SalesInvoiceService>>();

            _inventoryMock = new Mock<IInventoryService>();

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));


            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));


            _service = new SalesInvoiceService(
                _context,
                logger.Object,
                _inventoryMock.Object);
        }

        #endregion


        #region ========================= Add =========================

        [Fact]
        public async Task AddAsync_ShouldCreateInvoice_WhenDTOIsValid()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 20);

            var dto = CreateSalesInvoiceDTO(product.Id);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);


            var invoice =
                await _context.SalesInvoices
                .Include(x => x.Details)
                .FirstAsync();


            Assert.NotNull(invoice);
            Assert.Equal(InvoiceStatus.Unpaid, invoice.Status);
            Assert.Single(invoice.Details);
            Assert.StartsWith("SAL-", invoice.InvoiceNo);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateInvoiceWithPayment_WhenPaymentExists()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 20);

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 50,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.True(result.IsSuccess);


            var invoice =
                await _context.SalesInvoices
                .Include(x => x.Payments)
                .FirstAsync();


            Assert.Single(invoice.Payments);
            Assert.Equal(50, invoice.PaidAmount);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDTOIsNull()
        {
            // Act
            var result = await _service.AddAsync(null);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSalesDetailsRequired_WhenDetailsAreEmpty()
        {
            // Arrange
            var dto = new SalesInvoiceDTO
            {
                Details = new List<SalesInvoiceDetailDTO>()
            };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.SalesDetailsRequired,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDuplicateItem_WhenSameProductExistsTwice()
        {
            // Arrange
            var product = await CreateProductEntity();


            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.Add(
                new SalesInvoiceDetailDTO
                {
                    ItemId = product.Id,
                    ItemType = SaleItemType.Product,
                    Quantity = 1,
                    UnitPrice = 100
                });


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.DuplicateItemInInvoice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.First().Quantity = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidQuantity,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidUnitPrice_WhenPriceIsZero()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.First().UnitPrice = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidUnitPrice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMemberNotFound_WhenMemberDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.MemberId = 999;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.MemberNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
        {
            // Arrange

            var dto = CreateSalesInvoiceDTO(999);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.ProductNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSubscriptionNotFound_WhenSubscriptionDoesNotExist()
        {
            // Arrange

            var dto = new SalesInvoiceDTO
            {
                Details = new List<SalesInvoiceDetailDTO>
            {
                new()
                {
                    ItemId = 999,
                    ItemType = SaleItemType.Subscription,
                    Quantity = 1,
                    UnitPrice = 100
                }
            }
            };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.SubscriptionPlanNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentAmount_WhenPaymentIsNegative()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = -10,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentAmount,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentDate_WhenDateIsDefault()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 20,
                PaymentDate = default
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentDate,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPaymentExceedsTotal_WhenPaymentIsHigher()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);


            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 9999,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.PaymentExceedsInvoiceTotal,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInventoryError_WhenDecreaseStockFails()
        {
            // Arrange

            var product = await CreateProductEntity();


            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(
                    It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(
                    Result<bool>.Failure(
                        ResultCodes.InsufficientStock,
                        400));


            var dto = CreateSalesInvoiceDTO(product.Id);


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.InsufficientStock,
                result.Code);
        }

        #endregion


        #region ========================= Get All =========================

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInvoices()
        {
            // Arrange
            var member = await CreateMemberEntity();

            var product1 = await CreateProductEntity();
            var product2 = await CreateProductEntity();

            await CreateSalesInvoiceEntity(member.Id, product1.Id);
            await CreateSalesInvoiceEntity(member.Id, product2.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoInvoicesExist()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnInvoiceWithMember()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(member.Id, product.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);

            var invoice = result.Data.Single();

            Assert.NotNull(invoice.Member);
            Assert.Equal(member.Id, invoice.Member.Id);
            Assert.Equal(member.FullName, invoice.Member.FullName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnInvoiceWithDetails()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);

            var invoice = result.Data.Single();

            Assert.Single(invoice.Details);

            var detail = invoice.Details.First();

            Assert.Equal(product.Id, detail.ItemId);
            Assert.Equal(SaleItemType.Product, detail.ItemType);
            Assert.Equal(product.NameEn, detail.Product.NameEn);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnInvoiceWithPayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id,
                withPayment: true);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);

            var invoice = result.Data.Single();

            Assert.Single(invoice.Payments);
            Assert.Equal(50m, invoice.Payments.First().Amount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldLoadProductInformation()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            var detail = result.Data.Single().Details.Single();

            Assert.NotNull(detail.Product);
            Assert.Equal(product.NameEn, detail.Product.NameEn);
            Assert.Equal(product.Code, detail.Product.Code);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMultipleInvoicesOrderedFromDatabase()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(null, product.Id);
            await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());

            Assert.All(result.Data, x =>
            {
                Assert.NotEmpty(x.InvoiceNo);
                Assert.NotEmpty(x.Details);
            });
        }

        #endregion


        #region ========================= Get By Id =========================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoice_WhenInvoiceExists()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data);
            Assert.Equal(invoice.Id, result.Data.Id);
            Assert.Equal(invoice.InvoiceNo, result.Data.InvoiceNo);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMember_WhenInvoiceHasMember()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data.Member);
            Assert.Equal(member.Id, result.Data.Member.Id);
            Assert.Equal(member.FullName, result.Data.Member.FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDetails()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data.Details);

            var detail = result.Data.Details.First();

            Assert.Equal(product.Id, detail.ItemId);
            Assert.Equal(2, detail.Quantity);
            Assert.Equal(100, detail.UnitPrice);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldLoadProductInformation()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            var detail = result.Data.Details.Single();

            Assert.NotNull(detail.Product);
            Assert.Equal(product.Id, detail.Product.Id);
            Assert.Equal(product.NameEn, detail.Product.NameEn);
            Assert.Equal(product.Code, detail.Product.Code);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: true);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data.Payments);

            Assert.Equal(
                50m,
                result.Data.Payments.First().Amount);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoiceWithoutMember_WhenMemberIsNull()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Null(result.Data.Member);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldMapInvoiceTotalsCorrectly()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: true);

            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Equal(invoice.NetAmount, result.Data.NetAmount);
            Assert.Equal(invoice.PaidAmount, result.Data.PaidAmount);
            Assert.Equal(invoice.RemainingBalance, result.Data.RemainingBalance);
            Assert.Equal(invoice.Status, result.Data.Status);
        }

        #endregion


        #region ========================= Search =========================

        [Fact]
        public async Task SearchAsync_ShouldReturnAllInvoices_WhenSearchIsEmpty()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(null, product.Id);
            await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.SearchAsync(string.Empty);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoice_WhenInvoiceNumberMatches()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.InvoiceNo = "SAL-2026-07-000123";

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchAsync("000123");

            // Assert
            Assert.True(result.IsSuccess);

            var item = Assert.Single(result.Data);

            Assert.Equal(invoice.Id, item.Id);
            Assert.Equal(invoice.InvoiceNo, item.InvoiceNo);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoice_WhenNotesMatch()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Notes = "Paid using Visa";

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchAsync("Visa");

            // Assert
            Assert.True(result.IsSuccess);

            var item = Assert.Single(result.Data);

            Assert.Equal(invoice.Id, item.Id);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoice_WhenMemberNameMatches()
        {
            // Arrange
            var member = await CreateMemberEntity();
            member.FullName = "Mohammed Ahmed";

            await _context.SaveChangesAsync();

            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(member.Id, product.Id);

            // Act
            var result = await _service.SearchAsync("Mohammed");

            // Assert
            Assert.True(result.IsSuccess);

            var item = Assert.Single(result.Data);

            Assert.Equal(invoice.Id, item.Id);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnEmpty_WhenNothingMatches()
        {
            // Arrange
            var product = await CreateProductEntity();

            await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.SearchAsync("XYZ123");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMaximumTwentyInvoices()
        {
            // Arrange
            var product = await CreateProductEntity();

            for (int i = 0; i < 30; i++)
            {
                await CreateSalesInvoiceEntity(null, product.Id);
            }

            // Act
            var result = await _service.SearchAsync(string.Empty);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(20, result.Data.Count());
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoicesOrderedByInvoiceDateDescending()
        {
            // Arrange
            var product = await CreateProductEntity();

            var oldInvoice = await CreateSalesInvoiceEntity(null, product.Id);
            oldInvoice.InvoiceDate = DateTime.UtcNow.AddDays(-5);

            var newInvoice = await CreateSalesInvoiceEntity(null, product.Id);
            newInvoice.InvoiceDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchAsync(string.Empty);

            // Assert
            Assert.True(result.IsSuccess);

            var invoices = result.Data.ToList();

            Assert.Equal(newInvoice.Id, invoices.First().Id);
            Assert.Equal(oldInvoice.Id, invoices.Last().Id);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnSearchDTOOnly()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);

            // Act
            var result = await _service.SearchAsync(invoice.InvoiceNo);

            // Assert
            Assert.True(result.IsSuccess);

            var dto = Assert.Single(result.Data);

            Assert.Equal(invoice.Id, dto.Id);
            Assert.Equal(invoice.InvoiceNo, dto.InvoiceNo);
            Assert.Equal(invoice.NetAmount, dto.NetAmount);
            Assert.Equal(invoice.PaidAmount, dto.PaidAmount);
            Assert.Equal(invoice.RemainingBalance, dto.RemainingBalance);
            Assert.Equal(invoice.Status, dto.Status);
        }

        #endregion


        #region ========================= Get SalesInvoice Add/Update DTO =========================

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnLookupLists_WhenIdIsNull()
        {
            // Arrange
            await CreateMemberEntity();
            await CreateProductEntity();
            await CreateSubscriptionTypeEntity();
            await CreateCategoryEntity();

            // Act
            var result = await _service.GetSalesInvoiceAddUpdateDTO();

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data);
            Assert.Null(result.Data.SalesInvoice);

            Assert.Single(result.Data.Members);
            Assert.Single(result.Data.Products);
            Assert.Single(result.Data.SubscriptionTypes);
            Assert.Single(result.Data.Categories);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnInvoice_WhenInvoiceExists()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id,
                    withPayment: true);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data.SalesInvoice);
            Assert.Equal(invoice.Id, result.Data.SalesInvoice.Id);
            Assert.Equal(invoice.InvoiceNo, result.Data.SalesInvoice.InvoiceNo);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadInvoiceDetails()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            var detail =
                Assert.Single(result.Data.SalesInvoice.Details);

            Assert.Equal(product.Id, detail.ItemId);
            Assert.Equal(SaleItemType.Product, detail.ItemType);
            Assert.Equal(product.NameEn, detail.Product.NameEn);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadPayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: true);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data.SalesInvoice.Payments);
            Assert.Equal(
                50m,
                result.Data.SalesInvoice.Payments.First().Amount);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadMember()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data.SalesInvoice.Member);
            Assert.Equal(member.Id, result.Data.SalesInvoice.Member.Id);
            Assert.Equal(member.FullName, result.Data.SalesInvoice.Member.FullName);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnAllLookupLists()
        {
            // Arrange
            await CreateMemberEntity();
            await CreateProductEntity();
            await CreateSubscriptionTypeEntity();
            await CreateCategoryEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    (await _context.Products.FirstAsync()).Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotEmpty(result.Data.Members);
            Assert.NotEmpty(result.Data.Products);
            Assert.NotEmpty(result.Data.SubscriptionTypes);
            Assert.NotEmpty(result.Data.Categories);
        }

        #endregion


        #region ========================= Cancel =========================

        [Fact]
        public async Task CancelAsync_ShouldCancelInvoice_WhenInvoiceExists()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id);

            // Act
            var result = await _service.CancelAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(ResultCodes.UpdatedSuccessfully, result.Code);

            var updated = await _context.SalesInvoices.FindAsync(invoice.Id);

            Assert.Equal(InvoiceStatus.Cancelled, updated.Status);
            Assert.NotNull(updated.UpdatedAt);

            _inventoryMock.Verify(x =>
                x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        [Fact]
        public async Task CancelAsync_ShouldAppendReason_WhenReasonProvided()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id);

            invoice.Notes = "Original Notes";

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.CancelAsync(
                invoice.Id,
                "Customer requested cancellation");

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.SalesInvoices.FindAsync(invoice.Id);

            Assert.Contains("Original Notes", updated.Notes);
            Assert.Contains("Customer requested cancellation", updated.Notes);
        }

        [Fact]
        public async Task CancelAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result = await _service.CancelAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);

            _inventoryMock.Verify(x =>
                x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAsync_ShouldReturnAlreadyCancelled_WhenInvoiceAlreadyCancelled()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id);

            invoice.Status = InvoiceStatus.Cancelled;

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.CancelAsync(invoice.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.AlreadyCancelled, result.Code);
            Assert.Equal(400, result.StatusCode);

            _inventoryMock.Verify(x =>
                x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAsync_ShouldReturnInventoryError_WhenRestoreStockFails()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id);

            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.ProductNotFound,
                    400));

            // Act
            var result = await _service.CancelAsync(invoice.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.ProductNotFound, result.Code);
            Assert.Equal(400, result.StatusCode);

            var dbInvoice = await _context.SalesInvoices.FindAsync(invoice.Id);

            Assert.NotEqual(InvoiceStatus.Cancelled, dbInvoice.Status);
        }

        [Fact]
        public async Task CancelAsync_ShouldCallIncreaseStockOnce()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                memberId: null,
                productId: product.Id);

            // Act
            await _service.CancelAsync(invoice.Id);

            // Assert
            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(
                    It.Is<IEnumerable<StockMovementDTO>>(m =>
                        m.Count() == 1 &&
                        m.First().ProductId == product.Id &&
                        m.First().Quantity == 2)),
                Times.Once);
        }

        #endregion


        #region ========================= Update =========================

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInvoice_WhenDataIsValid()
        {
            // Arrange
            var product1 = await CreateProductEntity(quantity: 20);
            var product2 = await CreateProductEntity(quantity: 20);

            var invoice = await CreateSalesInvoiceEntity(null, product1.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product2.Id);
            dto.Notes = "Updated Notes";

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(ResultCodes.UpdatedSuccessfully, result.Code);

            var updated = await _context.SalesInvoices
                .Include(x => x.Details)
                .Include(x => x.Payments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Equal("Updated Notes", updated.Notes);
            Assert.NotNull(updated.UpdatedAt);
            Assert.Single(updated.Details);

            _inventoryMock.Verify(x =>
                x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);

            _inventoryMock.Verify(x =>
                x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            // Act
            var result = await _service.UpdateAsync(999, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidData_WhenDtoIsNull()
        {
            // Act
            var result = await _service.UpdateAsync(1, null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSalesDetailsRequired_WhenDetailsAreEmpty()
        {
            // Arrange
            var dto = new SalesInvoiceDTO
            {
                Details = new List<SalesInvoiceDetailDTO>()
            };

            // Act
            var result = await _service.UpdateAsync(1, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.SalesDetailsRequired, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnCannotEditPostedInvoice_WhenInvoiceIsNotDraft()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Status = InvoiceStatus.Paid;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.CannotEditPostedInvoice, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInventoryError_WhenRestoreStockFails()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.ProductNotFound,
                    400));

            var dto = CreateSalesInvoiceDTO(product.Id);

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.ProductNotFound, result.Code);

            _inventoryMock.Verify(x =>
                x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInventoryError_WhenDecreaseStockFails()
        {
            // Arrange
            var product1 = await CreateProductEntity(quantity: 20);
            var product2 = await CreateProductEntity(quantity: 20);

            var invoice = await CreateSalesInvoiceEntity(null, product1.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.InsufficientStock,
                    HttpStatusCodes.BadRequest));

            var dto = CreateSalesInvoiceDTO(product2.Id);

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);
            Assert.Equal(HttpStatusCodes.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceInvoiceDetails()
        {
            // Arrange
            var product1 = await CreateProductEntity();
            var product2 = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product1.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product2.Id);

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.SalesInvoices
                .Include(x => x.Details)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Single(updated.Details);
            Assert.Equal(product2.Id, updated.Details.First().ItemId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplacePayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                null,
                product.Id,
                withPayment: true);

            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);
            dto.Payments = new List<SalesPaymentDTO>
    {
        new()
        {
            Amount = 75,
            PaymentDate = DateTime.UtcNow
        }
    };

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.SalesInvoices
                .Include(x => x.Payments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Single(updated.Payments);
            Assert.Equal(75m, updated.Payments.First().Amount);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInvoiceFields()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);
            dto.MemberId = member.Id;
            dto.InvoiceDate = DateTime.UtcNow.AddDays(-3);
            dto.Notes = "Updated invoice";

            // Act
            await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            var updated = await _context.SalesInvoices.FindAsync(invoice.Id);

            Assert.Equal(member.Id, updated.MemberId);
            Assert.Equal(dto.InvoiceDate, updated.InvoiceDate);
            Assert.Equal("Updated invoice", updated.Notes);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldAllowEmptyPayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(
                null,
                product.Id,
                withPayment: true);

            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);
            dto.Payments = new List<SalesPaymentDTO>();

            // Act
            var result = await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.SalesInvoices
                .Include(x => x.Payments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Empty(updated.Payments);
        }

        [Fact]
        public async Task UpdateAsync_ShouldKeepStatusDraft()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);

            // Act
            await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            var updated = await _context.SalesInvoices.FindAsync(invoice.Id);

            Assert.Equal(InvoiceStatus.Draft, updated.Status);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallInventoryMethodsOnce()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice = await CreateSalesInvoiceEntity(null, product.Id);
            invoice.Status = InvoiceStatus.Draft;

            await _context.SaveChangesAsync();

            var dto = CreateSalesInvoiceDTO(product.Id);

            // Act
            await _service.UpdateAsync(invoice.Id, dto);

            // Assert
            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);

            _inventoryMock.Verify(
                x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        #endregion


        #region ========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteInvoice_WhenInvoiceExists()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: true);


            // Act
            var result = await _service.DeleteAsync(invoice.Id);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(
                ResultCodes.DeletedSuccessfully,
                result.Code);


            var deletedInvoice =
                await _context.SalesInvoices
                .IgnoreQueryFilters()
                .Include(x => x.Details)
                .Include(x => x.Payments)
                .FirstAsync(x => x.Id == invoice.Id);


            Assert.True(deletedInvoice.IsDeleted);
            Assert.NotNull(deletedInvoice.DeletedAt);
            Assert.NotNull(deletedInvoice.UpdatedAt);


            Assert.All(deletedInvoice.Details, detail =>
            {
                Assert.True(detail.IsDeleted);
                Assert.NotNull(detail.DeletedAt);
                Assert.NotNull(detail.UpdatedAt);
            });


            Assert.All(deletedInvoice.Payments, payment =>
            {
                Assert.True(payment.IsDeleted);
                Assert.NotNull(payment.DeletedAt);
                Assert.NotNull(payment.UpdatedAt);
            });
        }


        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result =
                await _service.DeleteAsync(999);


            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);

            Assert.Equal(
                404,
                result.StatusCode);


            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(
                    It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Never);
        }


        [Fact]
        public async Task DeleteAsync_ShouldRestoreStockBeforeDeleting()
        {
            // Arrange
            var product =
                await CreateProductEntity();


            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);


            // Act
            await _service.DeleteAsync(invoice.Id);


            // Assert
            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(
                    It.Is<IEnumerable<StockMovementDTO>>(m =>
                        m.Count() == 1 &&
                        m.First().ProductId == product.Id &&
                        m.First().Quantity == 2)),
                Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ShouldReturnInventoryError_WhenRestoreStockFails()
        {
            // Arrange
            var product =
                await CreateProductEntity();


            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);


            _inventoryMock
                .Setup(x =>
                    x.IncreaseStockAsync(
                        It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(
                    Result<bool>.Failure(
                        ResultCodes.ProductNotFound,
                        400));


            // Act
            var result =
                await _service.DeleteAsync(invoice.Id);


            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.ProductNotFound,
                result.Code);


            var dbInvoice =
                await _context.SalesInvoices
                .IgnoreQueryFilters()
                .FirstAsync(x => x.Id == invoice.Id);


            Assert.False(dbInvoice.IsDeleted);
        }


        [Fact]
        public async Task DeleteAsync_ShouldNotDeleteInvoice_WhenInventoryFails()
        {
            // Arrange
            var product =
                await CreateProductEntity();


            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);


            _inventoryMock
                .Setup(x =>
                    x.IncreaseStockAsync(
                        It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(
                    Result<bool>.Failure(
                        ResultCodes.InsufficientStock,
                        400));


            // Act
            await _service.DeleteAsync(invoice.Id);


            // Assert
            var dbInvoice =
                await _context.SalesInvoices
                .IgnoreQueryFilters()
                .FirstAsync(x => x.Id == invoice.Id);


            Assert.False(dbInvoice.IsDeleted);
        }


        [Fact]
        public async Task DeleteAsync_ShouldHandleInvoiceWithoutPayments()
        {
            // Arrange
            var product =
                await CreateProductEntity();


            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: false);


            // Act
            var result =
                await _service.DeleteAsync(invoice.Id);


            // Assert
            Assert.True(result.IsSuccess);


            var deleted =
                await _context.SalesInvoices
                .IgnoreQueryFilters()
                .FirstAsync(x => x.Id == invoice.Id);


            Assert.True(deleted.IsDeleted);
        }


        #endregion


        #region ========================= Helpers =========================


        private async Task<Product> CreateProductEntity(
            int quantity = 10)
        {
            var product = new Product
            {
                NameEn = "Test Product",
                NameAr = "منتج",
                Code = Guid.NewGuid().ToString(),
                PurchasePrice = 50,
                SalePrice = 100,
                Quantity = quantity
            };


            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }



        private async Task<Member> CreateMemberEntity()
        {
            var member = new Member
            {
                FullName = "Test Member",
                PhoneNumber = "0999999999"
            };


            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return member;
        }



        private SalesInvoiceDTO CreateSalesInvoiceDTO(
            int productId)
        {
            return new SalesInvoiceDTO
            {
                InvoiceDate = DateTime.UtcNow,

                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = productId,
                ItemType = SaleItemType.Product,
                Quantity = 2,
                UnitPrice = 100
            }
        },

                Payments = new List<SalesPaymentDTO>()
            };
        }

        private async Task<SalesInvoice> CreateSalesInvoiceEntity(
    int? memberId,
    int productId,
    bool withPayment = false)
        {
            var invoice = new SalesInvoice
            {
                InvoiceNo = Guid.NewGuid().ToString(),
                InvoiceDate = DateTime.UtcNow,
                MemberId = memberId,
                Status = InvoiceStatus.Unpaid,
                NetAmount = 200,
                PaidAmount = withPayment ? 50 : 0,
                RemainingBalance = withPayment ? 150 : 200,

                Details = new List<SalesInvoiceDetail>
        {
            new()
            {
                ItemId = productId,
                ItemType = SaleItemType.Product,
                Quantity = 2,
                UnitPrice = 100
            }
        },

                Payments = withPayment
                    ? new List<SalesPayment>
                    {
                new()
                {
                    Amount = 50,
                    PaymentDate = DateTime.UtcNow
                }
                    }
                    : new List<SalesPayment>()
            };

            _context.SalesInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        private async Task<SubscriptionType> CreateSubscriptionTypeEntity()
        {
            var entity = new SubscriptionType
            {
                NameEn = "Monthly",
                NameAr = "شهري",
                DurationDays = 30,
                Price = 100
            };

            _context.SubscriptionTypes.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        private async Task<Category> CreateCategoryEntity()
        {
            var entity = new Category
            {
                NameEn = "Supplements",
                NameAr = "المكملات"
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        #endregion

    }
}
