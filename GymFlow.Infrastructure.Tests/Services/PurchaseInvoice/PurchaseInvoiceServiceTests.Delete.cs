using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
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

        #region ========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteInvoice_WhenInvoiceExists()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);


            // Act
            var result = await _service.DeleteAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var deletedInvoice =
                await _context.PurchaseInvoices
                .IgnoreQueryFilters()
                .Include(x => x.PurchaseDetails)
                .Include(x => x.PurchasePayments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.True(deletedInvoice.IsDeleted);
            Assert.NotNull(deletedInvoice.DeletedAt);
            Assert.NotNull(deletedInvoice.UpdatedAt);

            Assert.All(
                deletedInvoice.PurchaseDetails,
                detail =>
                {
                    Assert.True(detail.IsDeleted);
                    Assert.NotNull(detail.DeletedAt);
                    Assert.NotNull(detail.UpdatedAt);
                });

            Assert.All(
                deletedInvoice.PurchasePayments,
                payment =>
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
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);

            Assert.Equal(
                HttpStatusCodes.NotFound,
                result.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDecreaseStock()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.DeleteAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            _inventoryMock.Verify(
                x => x.DecreaseStockAsync(
                    It.Is<IEnumerable<StockMovementDTO>>(m =>
                        m.Count() == 1 &&
                        m.First().ProductId == product.Id &&
                        m.First().Quantity == 2)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenDecreaseStockFails()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.InsufficientStock,
                    HttpStatusCodes.BadRequest));

            // Act
            var result = await _service.DeleteAsync(invoice.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);

            _inventoryMock.Verify(
                x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        #endregion

    }
}
