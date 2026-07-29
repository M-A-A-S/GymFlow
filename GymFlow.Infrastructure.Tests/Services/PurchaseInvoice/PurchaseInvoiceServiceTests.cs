using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.DTOs.PurchasePayment;
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


namespace GymFlow.Infrastructure.Tests.Services.PurchaseInvoice
{
    public partial class PurchaseInvoiceServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly PurchaseInvoiceService _service;
        private readonly Mock<IInventoryService> _inventoryMock;

        #endregion


        #region ========================= Constructors =========================

        public PurchaseInvoiceServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<PurchaseInvoiceService>>();

            _inventoryMock = new Mock<IInventoryService>();

            _inventoryMock
                .Setup(x => x.DecreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));


            _inventoryMock
                .Setup(x => x.IncreaseStockAsync(It.IsAny<IEnumerable<StockMovementDTO>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            _service = new PurchaseInvoiceService(
                _context,
                logger.Object, _inventoryMock.Object);
        }

        #endregion


        #region ========================= Helpers =========================

        private async Task<Supplier> CreateSupplierEntity()
        {
            var supplier = new Supplier
            {
                FullName = "Test Supplier",
                PhoneNumber = "0999999999",
                Address = "Khartoum"
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        private async Task<Product> CreateProductEntity()
        {
            var product = new Product
            {
                Code = Guid.NewGuid().ToString(),
                NameEn = "Test Product",
                NameAr = "منتج",
                PurchasePrice = 50,
                SalePrice = 70,
                Quantity = 100,
                ReorderLevel = 10
            };


            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private async Task<Domain.Entities.PurchaseInvoice> CreatePurchaseInvoiceEntity(
            int supplierId,
            int productId)
        {
            var invoice = new Domain.Entities.PurchaseInvoice
            {
                InvoiceNo = $"PUR-{Guid.NewGuid()}",
                SupplierId = supplierId,
                InvoiceDate = DateTime.UtcNow,
                Notes = "Test invoice",

                PurchaseDetails = new List<PurchaseDetail>
                {
                    new PurchaseDetail
                    {
                        ProductId = productId,
                        Quantity = 2,
                        UnitPrice = 50,
                        Total = 100
                    }
                },

                PurchasePayments = new List<PurchasePayment>
                {
                    new PurchasePayment
                    {
                        Amount = 100,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = PaymentMethod.Cash
                    }
                }
            };

            invoice.CalculateTotal();
            invoice.UpdatePaymentStatus();

            _context.PurchaseInvoices.Add(invoice);

            await _context.SaveChangesAsync();

            return invoice;
        }

        private PurchaseInvoiceDTO CreatePurchaseInvoiceDTO(
            int supplierId,
            int productId)
        {
            return new PurchaseInvoiceDTO
            {
                SupplierId = supplierId,
                InvoiceDate = DateTime.UtcNow,
                Notes = "Test invoice",
                PurchaseDetails = new List<PurchaseDetailDTO>
            {
                new PurchaseDetailDTO
                {
                    ProductId = productId,
                    Quantity = 2,
                    UnitPrice = 50
                }
            },

                PurchasePayments = new List<PurchasePaymentDTO>
            {
                new PurchasePaymentDTO
                {
                    Amount = 100,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = PaymentMethod.Cash
                }
            }
            };
        }


        #endregion
    
    
    }


}
