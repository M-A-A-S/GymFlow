using GymFlow.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.PurchaseInvoice
{
    public partial class PurchaseInvoiceServiceTests
    {

        #region ========================= Get =========================

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPurchaseInvoices_WithRelations()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);

            var invoices = result.Data.ToList();

            Assert.Equal(2, invoices.Count);

            Assert.NotNull(invoices.First().Supplier);
            Assert.NotEmpty(invoices.First().PurchaseDetails);
            Assert.NotEmpty(invoices.First().PurchasePayments);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoInvoicesExist()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoice_WhenIdExists()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);


            // Act
            var result = await _service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(
                invoice.InvoiceNo,
                result.Data.InvoiceNo);

            Assert.NotNull(result.Data.Supplier);
            Assert.Single(
                result.Data.PurchaseDetails);
            Assert.Single(
                result.Data.PurchasePayments);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

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
        public async Task SearchAsync_ShouldReturnInvoice_WhenInvoiceNumberMatches()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);


            invoice.InvoiceNo = "PUR-TEST-0001";

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchAsync("TEST");

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data);

            Assert.Equal(
                "PUR-TEST-0001",
                result.Data.First().InvoiceNo);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoice_WhenNotesMatches()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);


            invoice.Notes = "Urgent purchase order";

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchAsync("Urgent");


            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data);

            Assert.Equal(
                invoice.Id,
                result.Data.First().Id);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnInvoice_WhenSupplierNameMatches()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            supplier.FullName = "ABC Company";

            var product = await CreateProductEntity();

            await _context.SaveChangesAsync();

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.SearchAsync("ABC");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnAllInvoices_WhenSearchIsEmpty()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.SearchAsync("");

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Equal(
                2,
                result.Data.Count());
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnEmpty_WhenNoMatchFound()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.SearchAsync("NotFound");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        #endregion

    }
}
