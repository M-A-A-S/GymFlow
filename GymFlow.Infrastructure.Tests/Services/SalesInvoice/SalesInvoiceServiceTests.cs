using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Services;
using GymFlow.Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly Mock<IInventoryService> _inventoryMock;
        private readonly SalesInvoiceService _service;

        #endregion


        #region ========================= Constructors =========================
        public SalesInvoiceServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<SalesInvoiceService>>();

            _inventoryMock = new Mock<IInventoryService>();

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));


            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));


            _service = new SalesInvoiceService(
                _context,
                logger.Object,
                _inventoryMock.Object);
        }

        #endregion


        #region ========================= Helpers =========================


        private async Task<Product> CreateProductEntity(
            int quantity = 10)
        {
            var product = new Product
            {
                NameEn = "Test Product",
                NameAr = "منتج",
                Code = Guid.NewGuid().ToString(),
                PurchasePrice = 50,
                SalePrice = 100,
                Quantity = quantity
            };


            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }



        private async Task<Member> CreateMemberEntity()
        {
            var member = new Member
            {
                FullName = "Test Member",
                PhoneNumber = "0999999999"
            };


            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return member;
        }



        private SalesInvoiceDTO CreateSalesInvoiceDTO(
            int productId)
        {
            return new SalesInvoiceDTO
            {
                InvoiceDate = DateTime.UtcNow,

                Details = new List<SalesInvoiceDetailDTO>
        {
            new()
            {
                ItemId = productId,
                ItemType = SaleItemType.Product,
                Quantity = 2,
                UnitPrice = 100
            }
        },

                Payments = new List<SalesPaymentDTO>()
            };
        }

        private async Task<Domain.Entities.SalesInvoice> CreateSalesInvoiceEntity(
    int? memberId,
    int productId,
    bool withPayment = false)
        {
            var invoice = new Domain.Entities.SalesInvoice
            {
                InvoiceNo = Guid.NewGuid().ToString(),
                InvoiceDate = DateTime.UtcNow,
                MemberId = memberId,
                Status = InvoiceStatus.Unpaid,
                NetAmount = 200,
                PaidAmount = withPayment ? 50 : 0,
                RemainingBalance = withPayment ? 150 : 200,

                Details = new List<SalesInvoiceDetail>
        {
            new()
            {
                ItemId = productId,
                ItemType = SaleItemType.Product,
                Quantity = 2,
                UnitPrice = 100
            }
        },

                Payments = withPayment
                    ? new List<SalesPayment>
                    {
                new()
                {
                    Amount = 50,
                    PaymentDate = DateTime.UtcNow
                }
                    }
                    : new List<SalesPayment>()
            };

            _context.SalesInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        private async Task<SubscriptionType> CreateSubscriptionTypeEntity()
        {
            var entity = new SubscriptionType
            {
                NameEn = "Monthly",
                NameAr = "شهري",
                DurationDays = 30,
                Price = 100
            };

            _context.SubscriptionTypes.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        private async Task<Category> CreateCategoryEntity()
        {
            var entity = new Category
            {
                NameEn = "Supplements",
                NameAr = "المكملات"
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        #endregion

    }
}
