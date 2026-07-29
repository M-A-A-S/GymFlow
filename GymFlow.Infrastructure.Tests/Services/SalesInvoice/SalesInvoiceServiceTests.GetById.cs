using GymFlow.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

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

    }
}
