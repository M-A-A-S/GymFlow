using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Utilities;
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

    }
}
