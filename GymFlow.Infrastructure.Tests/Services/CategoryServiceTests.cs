using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.File;
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
    public class CategoryServiceTests
    {
        #region ========================= Fields =========================

        private readonly TestDbContext _context;
        private readonly CategoryService _service;
        private readonly Mock<IFileService> _fileServiceMock;

        #endregion

        #region ========================= Constructor =========================
        public CategoryServiceTests()
        {
            var options =
                new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new TestDbContext(options);


            var logger =
                new Mock<ILogger<CategoryService>>();


            _fileServiceMock =
                new Mock<IFileService>();


            _service =
                new CategoryService(
                    _context,
                    logger.Object,
                    _fileServiceMock.Object);
        }

        #endregion

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenDtoIsValid()
        {
            // Arrange

            var dto = CreateCategoryDTO();

            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.CategoriesFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "category.jpg",
                        ResultCodes.FileSaved));

            // Act
            var result =
                await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(
                ResultCodes.CreatedSuccessfully,
                result.Code);

            Assert.True(result.Data > 0);


            var entity =
                await _context.Categories
                    .FindAsync(result.Data);


            Assert.NotNull(entity);

            Assert.Equal(
                dto.NameEn,
                entity.NameEn);


            Assert.Equal(
                "category.jpg",
                entity.ImageUrl);

            _fileServiceMock.Verify(
                x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.CategoriesFolder,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenFileSaveFails()
        {
            // Arrange

            var dto = CreateCategoryDTO();



            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.CategoriesFolder,
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

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnCategories()
        {
            // Arrange

            await CreateCategoryEntity(
                "Fitness");


            await CreateCategoryEntity(
                "Nutrition");



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
        public async Task GetByIdAsync_ShouldReturnCategory_WhenExists()
        {
            // Arrange

            var category =
                await CreateCategoryEntity();



            // Act

            var result =
                await _service.GetByIdAsync(
                    category.Id);



            // Assert

            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data);

            Assert.Equal(
                category.NameEn,
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

            Assert.Equal(
                404,
                result.StatusCode);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingCategories()
        {
            // Arrange

            await CreateCategoryEntity(
                "Body Building");


            await CreateCategoryEntity(
                "Swimming");



            // Act

            var result =
                await _service.SearchAsync(
                    "Body");



            // Assert

            Assert.True(result.IsSuccess);

            Assert.Single(result.Data);

            Assert.Equal(
                "Body Building",
                result.Data.First().NameEn);
        }

        #endregion

        #region ========================= Update =========================
        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory_WhenValid()
        {
            // Arrange

            var category =
                await CreateCategoryEntity(
                    "Old Name");



            var dto =
                CreateCategoryDTO(
                    "New Name");



            _fileServiceMock
                .Setup(x => x.ReplaceAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    Constants.CategoriesFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "new.jpg"));



            // Act

            var result =
                await _service.UpdateAsync(
                    category.Id,
                    dto);



            // Assert

            Assert.True(result.IsSuccess);


            var updated =
                await _context.Categories
                    .FindAsync(category.Id);



            Assert.Equal(
                "New Name",
                updated.NameEn);

            Assert.NotNull(
                updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenCategoryMissing()
        {
            // Arrange

            var dto =
                CreateCategoryDTO();



            // Act

            var result =
                await _service.UpdateAsync(
                    999,
                    dto);



            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteCategory_WhenExists()
        {
            // Arrange

            var category =
                await CreateCategoryEntity();



            _fileServiceMock
                .Setup(x => x.DeleteAsync(
                    It.IsAny<string>(),
                    Constants.CategoriesFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<bool>.Success(true));



            // Act

            var result =
                await _service.DeleteAsync(
                    category.Id);



            // Assert

            Assert.True(result.IsSuccess);

            var deleted =
                await _context.Categories
                    .FindAsync(category.Id);



            Assert.True(
                deleted.IsDeleted);


            Assert.NotNull(
                deleted.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenCategoryMissing()
        {
            // Act

            var result =
                await _service.DeleteAsync(999);



            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.NotFound,
                result.Code);
        }

        #endregion

        #region ========================= Helpers =========================
        private async Task<Category> CreateCategoryEntity(
            string name = "Gym Equipment")
        {
            var category = new Category
            {
                NameEn = name,
                NameAr = "Test Arabic",
                DescriptionEn = "Description",
                DescriptionAr = "وصف",
                ImageUrl = "image.jpg",
                IsActive = true
            };


            _context.Categories.Add(category);

            await _context.SaveChangesAsync();


            return category;
        }

        private CategoryDTO CreateCategoryDTO(
            string name = "Gym Equipment")
        {
            return new CategoryDTO
            {
                NameEn = name,
                NameAr = "Test Arabic",
                DescriptionEn = "Description",
                DescriptionAr = "وصف",
                ImageUrl = null,
                IsActive = true
            };
        }

        #endregion
    }
}
