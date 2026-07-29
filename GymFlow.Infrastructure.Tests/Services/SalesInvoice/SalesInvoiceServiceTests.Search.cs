using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

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

    }
}
