using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.File;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.Entities;
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

namespace GymFlow.Infrastructure.Tests.Services
{
    public class ProductServiceTests
    {
        #region ========================= Fields =========================
        private readonly TestDbContext _context;
        private readonly ProductService _service;
        private readonly Mock<IFileService> _fileServiceMock;

        #endregion

        #region ========================= Constructor =========================
        public ProductServiceTests()
        {
            var options =
                new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger =
                new Mock<ILogger<ProductService>>();

            _fileServiceMock =
                new Mock<IFileService>();

            _service =
                new ProductService(
                    _context,
                    logger.Object,
                    _fileServiceMock.Object);
        }

        #endregion

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenProductIsValid()
        {
            // Arrange

            var dto = CreateProductDTO();

            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.ProductsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "product.jpg",
                        ResultCodes.FileSaved));

            // Act
            var result =
                await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);

            var entity =
                await _context.Products
                    .FindAsync(result.Data);

            Assert.NotNull(entity);
            Assert.Equal(
                dto.NameEn,
                entity.NameEn);
            Assert.Equal(
                "product.jpg",
                entity.ImageUrl);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenImageSaveFails()
        {
            // Arrange

            var dto = CreateProductDTO();

            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.ProductsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Failure(
                        ResultCodes.FileSaveFailed,
                        500));

            // Act
            var result =
                await _service.AddAsync(dto);

            // Assert

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenNameEnExists()
        {
            // Arrange

            await CreateProductEntity(
                "Protein");

            var dto =
                CreateProductDTO(
                    "Protein");

            // Act

            var result =
                await _service.AddAsync(dto);

            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NameEnAlreadyExists,
                result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenNameArExists()
        {
            // Arrange

            var product =
                await CreateProductEntity();

            var dto =
                CreateProductDTO();
            dto.NameEn = "test";

            dto.NameAr =
                product.NameAr;

            // Act

            var result =
                await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.NameArAlreadyExists,
                result.Code);
        }

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            // Arrange
            await CreateProductEntity(
                "Protein");

            await CreateProductEntity(
                "Creatine");

            // Act
            var result =
                await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(
                2,
                result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var product =
                await CreateProductEntity();

            // Act
            var result =
                await _service.GetByIdAsync(
                    product.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(
                product.NameEn,
                result.Data.NameEn);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenMissing()
        {
            // Act
            var result =
                await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnProductsByName()
        {
            // Arrange

            await CreateProductEntity(
                "Protein Powder");

            await CreateProductEntity(
                "T-Shirt");

            // Act

            var result =
                await _service.SearchAsync(
                    "Protein");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal(
                "Protein Powder",
                result.Data.First().NameEn);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnProductsByCode()
        {
            // Arrange
            await CreateProductEntity(
                "Protein",
                "PRD-100");

            // Act
            var result =
                await _service.SearchAsync(
                    "PRD-100");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
        }

        #endregion

        #region ========================= Update =========================
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenValid()
        {
            // Arrange

            var product =
                await CreateProductEntity(
                    "Old Name");

            var dto =
                CreateProductDTO(
                    "New Name");

            _fileServiceMock
                .Setup(x => x.ReplaceAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    Constants.ProductsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "new.jpg"));

            // Act
            var result =
                await _service.UpdateAsync(
                    product.Id,
                    dto);

            // Assert
            Assert.True(result.IsSuccess);
            var updated =
                await _context.Products
                    .FindAsync(product.Id);

            Assert.Equal(
                "New Name",
                updated.NameEn);

            Assert.Equal(
                "new.jpg",
                updated.ImageUrl);
            Assert.NotNull(
                updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenProductMissing()
        {
            var result =
                await _service.UpdateAsync(
                    999,
                    CreateProductDTO());


            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldFail_WhenNameExists()
        {
            // Arrange

            await CreateProductEntity(
                "Existing");

            var product =
                await CreateProductEntity(
                    "Old");

            var dto =
                CreateProductDTO(
                    "Existing");


            // Act

            var result =
                await _service.UpdateAsync(
                    product.Id,
                    dto);

            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NameEnAlreadyExists,
                result.Code);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteProduct()
        {
            // Arrange
            var product =
                await CreateProductEntity();

            _fileServiceMock
                .Setup(x => x.DeleteAsync(
                    It.IsAny<string>(),
                    Constants.ProductsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<bool>.Success(true));

            // Act
            var result =
                await _service.DeleteAsync(
                    product.Id);

            // Assert
            Assert.True(result.IsSuccess);

            var deleted =
                await _context.Products
                    .FindAsync(product.Id);

            Assert.True(
                deleted.IsDeleted);

            Assert.NotNull(
                deleted.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound()
        {
            var result =
                await _service.DeleteAsync(999);


            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);
        }



        #endregion

        #region ========================= Helpers =========================

        private async Task<Product> CreateProductEntity(
            string name = "Protein",
            string code = "PRD-000001")
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
                    Quantity = 5,
                    ReorderLevel = 2
                };


            _context.Products.Add(product);


            await _context.SaveChangesAsync();


            return product;
        }


        private ProductDTO CreateProductDTO(
            string name = "Protein")
        {
            return new ProductDTO
            {
                NameEn = name,
                NameAr = "منتج",
                DescriptionEn = "Description",
                DescriptionAr = "وصف",
                CategoryId = 1,
                PurchasePrice = 10,
                SalePrice = 20,
                Quantity = 5,
                ReorderLevel = 2,
                Code = "PRD-000002"
            };
        }


        #endregion
    }

}
