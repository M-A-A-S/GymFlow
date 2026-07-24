using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
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
    public class InventoryServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly InventoryService _service;

        #endregion

        #region ========================= Constructors =========================

        public InventoryServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<InventoryService>>();

            _service = new InventoryService(
                _context,
                logger.Object);
        }

        #endregion

        #region ========================= Increase Stock =========================

        [Fact]
        public async Task IncreaseStockAsync_ShouldIncreaseProductQuantity()
        {
            // Arrange
            var product = await CreateProductEntity();

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 5
            }
        };

            // Act
            var result = await _service.IncreaseStockAsync(items);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.Products.FindAsync(product.Id);

            Assert.Equal(15, updated.Quantity);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task IncreaseStockAsync_ShouldGroupDuplicateProducts()
        {
            // Arrange
            var product = await CreateProductEntity();

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 5
            },
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 10
            }
        };

            // Act
            var result = await _service.IncreaseStockAsync(items);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.Products.FindAsync(product.Id);

            Assert.Equal(25, updated.Quantity);
        }

        #endregion

        #region ========================= Decrease Stock =========================

        [Fact]
        public async Task DecreaseStockAsync_ShouldDecreaseQuantity_WhenStockIsEnough()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 20);

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 5
            }
        };

            // Act
            var result = await _service.DecreaseStockAsync(items);

            // Assert
            Assert.True(result.IsSuccess);

            var updated = await _context.Products.FindAsync(product.Id);

            Assert.Equal(15, updated.Quantity);
        }

        [Fact]
        public async Task DecreaseStockAsync_ShouldReturnInsufficientStock_WhenQuantityIsGreaterThanAvailable()
        {
            // Arrange
            var product = await CreateProductEntity(quantity: 5);

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 10
            }
        };

            // Act
            var result = await _service.DecreaseStockAsync(items);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InsufficientStock, result.Code);
            Assert.Equal(400, result.StatusCode);

            var updated = await _context.Products.FindAsync(product.Id);

            Assert.Equal(5, updated.Quantity);
        }

        #endregion

        #region ========================= Apply Stock Movements =========================

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldReturnSuccess_WhenItemsIsNull()
        {
            // Act
            var result = await _service.ApplyStockMovementsAsync(null);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldReturnSuccess_WhenItemsIsEmpty()
        {
            // Act
            var result = await _service.ApplyStockMovementsAsync(
                Enumerable.Empty<StockMovementDTO>());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldReturnInvalidStockQuantity_WhenQuantityIsZero()
        {
            // Arrange
            var product = await CreateProductEntity();

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 0,
                MovementType = StockMovementType.Increase
            }
        };

            // Act
            var result = await _service.ApplyStockMovementsAsync(items);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidStockQuantity, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = 999,
                Quantity = 5,
                MovementType = StockMovementType.Increase
            }
        };

            // Act
            var result = await _service.ApplyStockMovementsAsync(items);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.ProductNotFound, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldReturnInvalidMovementType_WhenMovementTypeIsInvalid()
        {
            // Arrange
            var product = await CreateProductEntity();

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product.Id,
                Quantity = 5,
                MovementType = (StockMovementType)100
            }
        };

            // Act
            var result = await _service.ApplyStockMovementsAsync(items);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidStockMovementType, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ApplyStockMovementsAsync_ShouldApplyIncreaseAndDecreaseForDifferentProducts()
        {
            // Arrange
            var product1 = await CreateProductEntity(quantity: 10, code: "PRD-000001");
            var product2 = await CreateProductEntity(quantity: 20, code: "PRD-000002");

            var items = new[]
            {
            new StockMovementDTO
            {
                ProductId = product1.Id,
                Quantity = 5,
                MovementType = StockMovementType.Increase
            },
            new StockMovementDTO
            {
                ProductId = product2.Id,
                Quantity = 8,
                MovementType = StockMovementType.Decrease
            }
        };

            // Act
            var result = await _service.ApplyStockMovementsAsync(items);

            // Assert
            Assert.True(result.IsSuccess);

            var updated1 = await _context.Products.FindAsync(product1.Id);
            var updated2 = await _context.Products.FindAsync(product2.Id);

            Assert.Equal(15, updated1.Quantity);
            Assert.Equal(12, updated2.Quantity);
        }

        #endregion

        #region ========================= Helpers =========================
        private async Task<Product> CreateProductEntity(
            string name = "Protein",
            string code = "PRD-000001",
            int quantity = 10)
        {
            var category =
                new Category
                {
                    NameEn = "Supplements",
                    NameAr = "مكملات"
                };


            _context.Categories.Add(category);


            var product =
                new Product
                {
                    Code = code,
                    NameEn = name,
                    NameAr = "منتج",
                    DescriptionEn = "Description",
                    DescriptionAr = "وصف",
                    ImageUrl = "image.jpg",
                    Category = category,
                    PurchasePrice = 10,
                    SalePrice = 20,
                    Quantity = quantity,
                    ReorderLevel = 2
                };


            _context.Products.Add(product);


            await _context.SaveChangesAsync();


            return product;
        }

        #endregion
    }

}
