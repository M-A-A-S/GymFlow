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

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

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

    }
}
