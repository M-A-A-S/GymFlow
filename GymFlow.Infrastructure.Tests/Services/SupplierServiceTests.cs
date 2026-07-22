using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Entities;
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
    public class SupplierServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly SupplierService _service;

        #endregion

        #region ========================= Constructors =========================
        public SupplierServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<SupplierService>>();

            _service = new SupplierService(
                _context,
                logger.Object);
        }

        #endregion

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenDtoIsValid()
        {
            // Arrange
            var dto = CreateSupplierDTO();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);

            var entity = await _context.Suppliers.FindAsync(result.Data);

            Assert.NotNull(entity);
            Assert.Equal(dto.FullName, entity.FullName);
            Assert.Equal(dto.PhoneNumber, entity.PhoneNumber);
            Assert.Equal(dto.Address, entity.Address);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPhoneExists_WhenPhoneAlreadyExists()
        {
            // Arrange
            await CreateSupplierEntity(phone: "0999999999");

            var dto = CreateSupplierDTO(phone: "0999999999");

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.PhoneExists, result.Code);
            Assert.Equal(409, result.StatusCode);
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

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSuppliers()
        {
            // Arrange
            await CreateSupplierEntity(phone: "111");
            await CreateSupplierEntity(phone: "222");

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSupplier_WhenSupplierExists()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            // Act
            var result = await _service.GetByIdAsync(supplier.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(supplier.FullName, result.Data.FullName);
            Assert.Equal(supplier.PhoneNumber, result.Data.PhoneNumber);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenSupplierDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnFilteredSuppliers()
        {
            // Arrange
            await CreateSupplierEntity(name: "Ali Ahmed", phone: "111");
            await CreateSupplierEntity(name: "Mohammed Hassan", phone: "222");

            // Act
            var result = await _service.SearchAsync("Ali");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("Ali Ahmed", result.Data.First().FullName);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnAllSuppliers_WhenSearchIsEmpty()
        {
            // Arrange
            await CreateSupplierEntity(phone: "111");
            await CreateSupplierEntity(phone: "222");

            // Act
            var result = await _service.SearchAsync("");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        #endregion

        #region ========================= Update =========================
        [Fact]
        public async Task UpdateAsync_ShouldUpdateSupplier_WhenDtoIsValid()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            var dto = CreateSupplierDTO(
                name: "Updated Supplier",
                phone: "0123456789");

            dto.Address = "Updated Address";

            // Act
            var result = await _service.UpdateAsync(supplier.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated = await _context.Suppliers.FindAsync(supplier.Id);

            Assert.Equal("Updated Supplier", updated.FullName);
            Assert.Equal("0123456789", updated.PhoneNumber);
            Assert.Equal("Updated Address", updated.Address);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenSupplierDoesNotExist()
        {
            // Arrange
            var dto = CreateSupplierDTO();

            // Act
            var result = await _service.UpdateAsync(999, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnPhoneExists_WhenPhoneBelongsToAnotherSupplier()
        {
            // Arrange
            await CreateSupplierEntity(name: "Supplier 1", phone: "111");

            var supplier2 = await CreateSupplierEntity(name: "Supplier 2", phone: "222");

            var dto = CreateSupplierDTO(
                name: "Updated",
                phone: "111");

            // Act
            var result = await _service.UpdateAsync(supplier2.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.PhoneExists, result.Code);
            Assert.Equal(409, result.StatusCode);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteSupplier()
        {
            // Arrange
            var supplier = await CreateSupplierEntity();

            // Act
            var result = await _service.DeleteAsync(supplier.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var entity = await _context.Suppliers.FindAsync(supplier.Id);

            Assert.True(entity.IsDeleted);
            Assert.NotNull(entity.UpdatedAt);
            Assert.NotNull(entity.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenSupplierDoesNotExist()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion

        #region ========================= Helpers =========================

        private async Task<Supplier> CreateSupplierEntity(
            string name = "Mohammed Alfatih",
            string phone = "0999888777")
        {
            var supplier = new Supplier
            {
                FullName = name,
                PhoneNumber = phone,
                Address = "Khartoum"
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        private SupplierDTO CreateSupplierDTO(
            string name = "Mohammed Alfatih",
            string phone = "0999888777")
        {
            return new SupplierDTO
            {
                FullName = name,
                PhoneNumber = phone,
                Address = "Khartoum"
            };
        }

        #endregion
    
    }

}
