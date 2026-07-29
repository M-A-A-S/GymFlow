using GymFlow.Domain.Constants;
using GymFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SalesInvoice
{
    public partial class SalesInvoiceServiceTests
    {

        #region ========================= Get SalesInvoice Add/Update DTO =========================

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnLookupLists_WhenIdIsNull()
        {
            // Arrange
            await CreateMemberEntity();
            await CreateProductEntity();
            await CreateSubscriptionTypeEntity();
            await CreateCategoryEntity();

            // Act
            var result = await _service.GetSalesInvoiceAddUpdateDTO();

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data);
            Assert.Null(result.Data.SalesInvoice);

            Assert.Single(result.Data.Members);
            Assert.Single(result.Data.Products);
            Assert.Single(result.Data.SubscriptionTypes);
            Assert.Single(result.Data.Categories);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnInvoice_WhenInvoiceExists()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id,
                    withPayment: true);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data.SalesInvoice);
            Assert.Equal(invoice.Id, result.Data.SalesInvoice.Id);
            Assert.Equal(invoice.InvoiceNo, result.Data.SalesInvoice.InvoiceNo);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadInvoiceDetails()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            var detail =
                Assert.Single(result.Data.SalesInvoice.Details);

            Assert.Equal(product.Id, detail.ItemId);
            Assert.Equal(SaleItemType.Product, detail.ItemType);
            Assert.Equal(product.NameEn, detail.Product.NameEn);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadPayments()
        {
            // Arrange
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    product.Id,
                    withPayment: true);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Single(result.Data.SalesInvoice.Payments);
            Assert.Equal(
                50m,
                result.Data.SalesInvoice.Payments.First().Amount);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldLoadMember()
        {
            // Arrange
            var member = await CreateMemberEntity();
            var product = await CreateProductEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    member.Id,
                    product.Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data.SalesInvoice.Member);
            Assert.Equal(member.Id, result.Data.SalesInvoice.Member.Id);
            Assert.Equal(member.FullName, result.Data.SalesInvoice.Member.FullName);
        }

        [Fact]
        public async Task GetSalesInvoiceAddUpdateDTO_ShouldReturnAllLookupLists()
        {
            // Arrange
            await CreateMemberEntity();
            await CreateProductEntity();
            await CreateSubscriptionTypeEntity();
            await CreateCategoryEntity();

            var invoice =
                await CreateSalesInvoiceEntity(
                    null,
                    (await _context.Products.FirstAsync()).Id);

            // Act
            var result =
                await _service.GetSalesInvoiceAddUpdateDTO(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.NotEmpty(result.Data.Members);
            Assert.NotEmpty(result.Data.Products);
            Assert.NotEmpty(result.Data.SubscriptionTypes);
            Assert.NotEmpty(result.Data.Categories);
        }

        #endregion

    }
}
