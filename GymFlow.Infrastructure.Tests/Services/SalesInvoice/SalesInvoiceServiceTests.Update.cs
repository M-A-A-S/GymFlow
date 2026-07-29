using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

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

    }
}
