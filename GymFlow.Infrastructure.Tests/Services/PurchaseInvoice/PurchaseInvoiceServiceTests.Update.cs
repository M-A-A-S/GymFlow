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

        #region ========================= Update =========================

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInvoice_WhenDtoIsValid()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.Notes = "Updated invoice note";
            dto.InvoiceDate = DateTime.UtcNow.AddDays(5);

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            Assert.Equal(
                ResultCodes.UpdatedSuccessfully,
                result.Code);


            var updatedInvoice =
                await _context.PurchaseInvoices
                    .Include(x => x.PurchaseDetails)
                    .Include(x => x.PurchasePayments)
                    .FirstAsync(x => x.Id == invoice.Id);

            Assert.Equal(
                "Updated invoice note",
                updatedInvoice.Notes);

            Assert.Single(
                updatedInvoice.PurchaseDetails);

            Assert.Single(
                updatedInvoice.PurchasePayments);


            Assert.NotNull(
                updatedInvoice.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceDetails_WhenUpdatingInvoice()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var oldProduct = await CreateProductEntity();
            var newProduct = await CreateProductEntity();
            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                oldProduct.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                newProduct.Id);

            dto.PurchaseDetails.First().Quantity = 5;

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.True(result.IsSuccess);

            var updatedInvoice =
                await _context.PurchaseInvoices
                .Include(x => x.PurchaseDetails)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Single(
                updatedInvoice.PurchaseDetails);

            Assert.Equal(
                newProduct.Id,
                updatedInvoice.PurchaseDetails.First().ProductId);

            Assert.Equal(
                5,
                updatedInvoice.PurchaseDetails.First().Quantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplacePayments_WhenUpdatingInvoice()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = 50;

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.True(result.IsSuccess);

            var updatedInvoice =
                await _context.PurchaseInvoices
                .Include(x => x.PurchasePayments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.Single(
                updatedInvoice.PurchasePayments);

            Assert.Equal(
                50,
                updatedInvoice.PurchasePayments.First().Amount);

            Assert.Equal(
                PaymentStatus.Partial,
                updatedInvoice.PaymentStatus);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.UpdateAsync(
                999,
                dto);

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
        public async Task UpdateAsync_ShouldReturnSupplierNotFound_WhenSupplierDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                999,
                product.Id);

            // Act
            var result = await _service.UpdateAsync(
                1,
                dto);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.SupplierNotFound,
                result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnDuplicateProduct_WhenDuplicateProductsExist()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchaseDetails.Add(
                new PurchaseDetailDTO
                {
                    ProductId = product.Id,
                    Quantity = 2,
                    UnitPrice = 10
                });


            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.DuplicateProductInInvoice,
                result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchaseDetails.First().Quantity = 0;

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidQuantity,
                result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnPaymentExceedsTotal_WhenPaymentTooHigh()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchasePayments.First().Amount = 9999;

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);



            // Assert
            Assert.False(result.IsSuccess);


            Assert.Equal(
                ResultCodes.PaymentExceedsInvoiceTotal,
                result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallDecreaseAndIncreaseStock()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            var oldProduct = await CreateProductEntity();
            var newProduct = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                oldProduct.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                newProduct.Id);

            dto.PurchaseDetails.First().Quantity = 5;

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.True(result.IsSuccess);

            _inventoryMock.Verify(
                x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);

            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenDecreaseStockFails()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.InsufficientStock,
                    HttpStatusCodes.BadRequest));

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);

            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenIncreaseStockFails()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            var oldProduct = await CreateProductEntity();
            var newProduct = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                oldProduct.Id);

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                newProduct.Id);

            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Failure(
                    ResultCodes.InsufficientStock,
                    HttpStatusCodes.BadRequest));

            // Act
            var result = await _service.UpdateAsync(
                invoice.Id,
                dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);

            _inventoryMock.Verify(
                x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);

            _inventoryMock.Verify(
                x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()),
                Times.Once);
        }

        #endregion

    }
}
