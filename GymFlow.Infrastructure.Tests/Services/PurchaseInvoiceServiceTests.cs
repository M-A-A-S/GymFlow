using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.DTOs.PurchasePayment;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
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

namespace GymFlow.Infrastructure.Tests.Services
{
    public class PurchaseInvoiceServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly PurchaseInvoiceService _service;

        #endregion


        #region ========================= Constructors =========================

        public PurchaseInvoiceServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<PurchaseInvoiceService>>();

            _service = new PurchaseInvoiceService(
                _context,
                logger.Object);
        }

        #endregion


        #region ========================= Add =========================

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenInvoiceIsValid()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);

            var invoice = await _context.PurchaseInvoices
                .Include(x => x.PurchaseDetails)
                .Include(x => x.PurchasePayments)
                .FirstOrDefaultAsync(x => x.Id == result.Data);


            Assert.NotNull(invoice);
            Assert.NotEmpty(invoice.InvoiceNo);
            Assert.StartsWith("PUR-", invoice.InvoiceNo);

            Assert.Equal(supplier.Id, invoice.SupplierId);

            Assert.Single(invoice.PurchaseDetails);
            Assert.Single(invoice.PurchasePayments);

            Assert.Equal(
                (dto.PurchaseDetails.First().Quantity *
                dto.PurchaseDetails.First().UnitPrice),
                invoice.TotalAmount);
        }

        [Fact]
        public async Task AddAsync_ShouldGenerateInvoiceNumber_WhenInvoiceIsAdded()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);


            Assert.NotNull(invoice);
            Assert.NotNull(invoice.InvoiceNo);
            Assert.Contains(DateTime.UtcNow.ToString("yyyy-MM"), invoice.InvoiceNo);
        }

        [Fact]
        public async Task AddAsync_ShouldSetPaymentStatusToPaid_WhenFullPaymentProvided()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = 100;

            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);


            Assert.Equal(
                PaymentStatus.Paid,
                invoice.PaymentStatus);
        }

        [Fact]
        public async Task AddAsync_ShouldSetPaymentStatusToPartial_WhenPartialPaymentProvided()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchasePayments.First().Amount = 20;


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            var invoice = await _context.PurchaseInvoices
                .FindAsync(result.Data);

            Assert.Equal(
                PaymentStatus.Partial,
                invoice.PaymentStatus);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDtoIsNull()
        {
            // Act
            var result = await _service.AddAsync(null);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSupplierNotFound_WhenSupplierDoesNotExist()
        {
            // Arrange
            var product = await CreateProductEntity();

            var dto = CreatePurchaseInvoiceDTO(
                999,
                product.Id);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.SupplierNotFound, result.Code);
            Assert.Equal(HttpStatusCodes.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPurchaseDetailsRequired_WhenDetailsAreEmpty()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                1);

            dto.PurchaseDetails.Clear();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.PurchaseDetailsRequired,
                result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDuplicateProduct_WhenSameProductAddedTwice()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);

            dto.PurchaseDetails.Add(
                new PurchaseDetailDTO
                {
                    ProductId = product.Id,
                    Quantity = 2,
                    UnitPrice = 50
                });


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.DuplicateProductInInvoice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchaseDetails.First().Quantity = 0;


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
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchaseDetails.First().UnitPrice = 0;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidUnitPrice,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                999);


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.ProductNotFound,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentAmount_WhenPaymentIsNegative()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = -10;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentAmount,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPaymentDate_WhenPaymentDateIsDefault()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().PaymentDate = default;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.InvalidPaymentDate,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPaymentExceedsInvoiceTotal_WhenPaymentIsGreater()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var dto = CreatePurchaseInvoiceDTO(
                supplier.Id,
                product.Id);


            dto.PurchasePayments.First().Amount = 5000;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.PaymentExceedsInvoiceTotal,
                result.Code);
        }

        #endregion


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


        #endregion


        #region ========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteInvoice_WhenInvoiceExists()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();
            var product = await CreateProductEntity();


            var invoice = await CreatePurchaseInvoiceEntity(
                supplier.Id,
                product.Id);


            // Act
            var result = await _service.DeleteAsync(invoice.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var deletedInvoice =
                await _context.PurchaseInvoices
                .IgnoreQueryFilters()
                .Include(x => x.PurchaseDetails)
                .Include(x => x.PurchasePayments)
                .FirstAsync(x => x.Id == invoice.Id);

            Assert.True(deletedInvoice.IsDeleted);
            Assert.NotNull(deletedInvoice.DeletedAt);
            Assert.NotNull(deletedInvoice.UpdatedAt);

            Assert.All(
                deletedInvoice.PurchaseDetails,
                detail =>
                {
                    Assert.True(detail.IsDeleted);
                    Assert.NotNull(detail.DeletedAt);
                    Assert.NotNull(detail.UpdatedAt);
                });

            Assert.All(
                deletedInvoice.PurchasePayments,
                payment =>
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
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);

            Assert.Equal(
                HttpStatusCodes.NotFound,
                result.StatusCode);
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

        private async Task<PurchaseInvoice> CreatePurchaseInvoiceEntity(
            int supplierId,
            int productId)
        {
            var invoice = new PurchaseInvoice
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
