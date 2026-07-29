using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.PurchaseInvoice
{
    public partial class PurchaseInvoiceServiceTests
    {

        #region ========================= Add =========================

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenInvoiceIsValid()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);

            var invoice = await _context.PurchaseInvoices
                .Include(x => x.PurchaseDetails)
                .Include(x => x.PurchasePayments)
                .FirstOrDefaultAsync(x => x.Id == result.Data);


            Assert.NotNull(invoice);
            Assert.NotEmpty(invoice.InvoiceNo);
            Assert.StartsWith("PUR-", invoice.InvoiceNo);

            Assert.Equal(supplier.Id, invoice.SupplierId);

            Assert.Single(invoice.PurchaseDetails);
            Assert.Single(invoice.PurchasePayments);

            Assert.Equal(
                (dto.PurchaseDetails.First().Quantity *
                dto.PurchaseDetails.First().UnitPrice),
                invoice.TotalAmount);
        }

        [Fact]
        public async Task AddAsync_ShouldGenerateInvoiceNumber_WhenInvoiceIsAdded()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);


            Assert.NotNull(invoice);
            Assert.NotNull(invoice.InvoiceNo);
            Assert.Contains(DateTime.UtcNow.ToString("yyyy-MM"), invoice.InvoiceNo);
        }

        [Fact]
        public async Task AddAsync_ShouldSetPaymentStatusToPaid_WhenFullPaymentProvided()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = 100;

            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);


            Assert.Equal(
                PaymentStatus.Paid,
                invoice.PaymentStatus);
        }

        [Fact]
        public async Task AddAsync_ShouldSetPaymentStatusToPartial_WhenPartialPaymentProvided()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchasePayments.First().Amount = 20;


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);

            Assert.Equal(
                PaymentStatus.Partial,
                invoice.PaymentStatus);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDtoIsNull()
        {
            // Act
            var result = await _service.AddAsync(null);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSupplierNotFound_WhenSupplierDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                999,
                product.Id);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.SupplierNotFound, result.Code);
            Assert.Equal(HttpStatusCodes.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPurchaseDetailsRequired_WhenDetailsAreEmpty()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                1);

            dto.PurchaseDetails.Clear();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.PurchaseDetailsRequired,
                result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDuplicateProduct_WhenSameProductAddedTwice()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchaseDetails.Add(
                new PurchaseDetailDTO
                {
                    ProductId = product.Id,
                    Quantity = 2,
                    UnitPrice = 50
                });


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.DuplicateProductInInvoice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchaseDetails.First().Quantity = 0;


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
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchaseDetails.First().UnitPrice = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidUnitPrice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                999);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.ProductNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentAmount_WhenPaymentIsNegative()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = -10;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentAmount,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentDate_WhenPaymentDateIsDefault()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().PaymentDate = default;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentDate,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPaymentExceedsInvoiceTotal_WhenPaymentIsGreater()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = 5000;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.PaymentExceedsInvoiceTotal,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldCallIncreaseStock_WhenInvoiceIsValid()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);

            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(
                    It.Is<IEnumerable<StockMovementDTO>>(m =>
                        m.Count() == 1 &&
                        m.First().ProductId == product.Id &&
                        m.First().Quantity == 2)),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenIncreaseStockFails()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.InsufficientStock,
                    HttpStatusCodes.BadRequest));

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);

            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        #endregion

    }
}
