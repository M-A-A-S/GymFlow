using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

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

    }
}
