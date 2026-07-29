using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Entities;
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

        #region ========================= Add =========================

        [Fact]
        public async Task AddAsync_ShouldCreateInvoice_WhenDTOIsValid()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 20);

            var dto = CreateSalesInvoiceDTO(product.Id);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);


            var invoice =
                await _context.SalesInvoices
                .Include(x => x.Details)
                .FirstAsync();


            Assert.NotNull(invoice);
            Assert.Equal(InvoiceStatus.Unpaid, invoice.Status);
            Assert.Single(invoice.Details);
            Assert.StartsWith("SAL-", invoice.InvoiceNo);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateInvoiceWithPayment_WhenPaymentExists()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 20);

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 50,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.True(result.IsSuccess);


            var invoice =
                await _context.SalesInvoices
                .Include(x => x.Payments)
                .FirstAsync();


            Assert.Single(invoice.Payments);
            Assert.Equal(50, invoice.PaidAmount);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDTOIsNull()
        {
            // Act
            var result = await _service.AddAsync(null);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSalesDetailsRequired_WhenDetailsAreEmpty()
        {
            // Arrange
            var dto = new SalesInvoiceDTO
            {
                Details = new List<SalesInvoiceDetailDTO>()
            };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.SalesDetailsRequired,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDuplicateItem_WhenSameProductExistsTwice()
        {
            // Arrange
            var product = await CreateProductEntity();


            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.Add(
                new SalesInvoiceDetailDTO
                {
                    ItemId = product.Id,
                    ItemType = SaleItemType.Product,
                    Quantity = 1,
                    UnitPrice = 100
                });


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.DuplicateItemInInvoice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.First().Quantity = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidQuantity,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidUnitPrice_WhenPriceIsZero()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Details.First().UnitPrice = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidUnitPrice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMemberNotFound_WhenMemberDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.MemberId = 999;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.MemberNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
        {
            // Arrange

            var dto = CreateSalesInvoiceDTO(999);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.ProductNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSubscriptionNotFound_WhenSubscriptionDoesNotExist()
        {
            // Arrange

            var dto = new SalesInvoiceDTO
            {
                Details = new List<SalesInvoiceDetailDTO>
            {
                new()
                {
                    ItemId = 999,
                    ItemType = SaleItemType.Subscription,
                    Quantity = 1,
                    UnitPrice = 100
                }
            }
            };


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.SubscriptionPlanNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentAmount_WhenPaymentIsNegative()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = -10,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentAmount,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentDate_WhenDateIsDefault()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);

            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 20,
                PaymentDate = default
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentDate,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPaymentExceedsTotal_WhenPaymentIsHigher()
        {
            // Arrange

            var product = await CreateProductEntity();

            var dto = CreateSalesInvoiceDTO(product.Id);


            dto.Payments = new List<SalesPaymentDTO>
        {
            new()
            {
                Amount = 9999,
                PaymentDate = DateTime.UtcNow
            }
        };


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.PaymentExceedsInvoiceTotal,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInventoryError_WhenDecreaseStockFails()
        {
            // Arrange

            var product = await CreateProductEntity();


            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(
                    It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(
                    Result<bool>.Failure(
                        ResultCodes.InsufficientStock,
                        400));


            var dto = CreateSalesInvoiceDTO(product.Id);


            // Act

            var result = await _service.AddAsync(dto);


            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.InsufficientStock,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSubscriptionStartDateRequired_WhenSubscriptionStartDateIsMissing()
        {
            // Arrange
            var subscription = await CreateSubscriptionTypeEntity();

            var member = await CreateMemberEntity();

            var dto = new SalesInvoiceDTO
            {
                MemberId = member.Id,
                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = subscription.Id,
                ItemType = SaleItemType.Subscription,
                Quantity = 1,
                UnitPrice = 100
            }
        }
            };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.SubscriptionStartDateRequired,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMemberAlreadyHasSubscription_WhenSubscriptionOverlaps()
        {
            // Arrange
            var member = await CreateMemberEntity();

            //var subscription = await CreateSubscriptionTypeEntity(durationDays: 30);
            var subscription = await CreateSubscriptionTypeEntity();

            _context.MemberSubscriptions.Add(new MemberSubscription
            {
                MemberId = member.Id,
                SubscriptionTypeId = subscription.Id,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
            });

            await _context.SaveChangesAsync();

            var dto = new SalesInvoiceDTO
            {
                MemberId = member.Id,
                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = subscription.Id,
                ItemType = SaleItemType.Subscription,
                Quantity = 1,
                UnitPrice = 100,
                SubscriptionStartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
            }
        }
            };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.MemberAlreadyHasSubscription,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateMemberSubscription_WhenInvoiceContainsSubscription()
        {
            // Arrange
            var member = await CreateMemberEntity();

            //var subscription = await CreateSubscriptionTypeEntity(durationDays: 30);
            var subscription = await CreateSubscriptionTypeEntity();

            var dto = new SalesInvoiceDTO
            {
                MemberId = member.Id,
                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = subscription.Id,
                ItemType = SaleItemType.Subscription,
                Quantity = 1,
                UnitPrice = 200,
                SubscriptionStartDate = DateOnly.FromDateTime(DateTime.Today)
            }
        }
            };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);

            var memberSubscription = await _context.MemberSubscriptions.FirstOrDefaultAsync();

            Assert.NotNull(memberSubscription);

            Assert.Equal(member.Id, memberSubscription.MemberId);

            Assert.Equal(subscription.Id, memberSubscription.SubscriptionTypeId);

            Assert.Equal(
                DateOnly.FromDateTime(DateTime.Today),
                memberSubscription.StartDate);

            Assert.Equal(
                DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                memberSubscription.EndDate);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateInvoice_WhenPreviousSubscriptionDoesNotOverlap()
        {
            // Arrange
            var member = await CreateMemberEntity();

            var subscription = await CreateSubscriptionTypeEntity();

            _context.MemberSubscriptions.Add(new MemberSubscription
            {
                MemberId = member.Id,
                SubscriptionTypeId = subscription.Id,
                StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-60)),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-30))
            });

            await _context.SaveChangesAsync();

            var dto = new SalesInvoiceDTO
            {
                MemberId = member.Id,
                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = subscription.Id,
                ItemType = SaleItemType.Subscription,
                Quantity = 1,
                UnitPrice = 100,
                SubscriptionStartDate = DateOnly.FromDateTime(DateTime.Today)
            }
        }
            };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        #endregion

    }
}
