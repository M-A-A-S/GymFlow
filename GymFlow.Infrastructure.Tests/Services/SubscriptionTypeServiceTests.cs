using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.SubscriptionType;
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
    public class SubscriptionTypeServiceTests
    {
        private readonly TestDbContext _context;
        private readonly SubscriptionTypeService _service;

        public SubscriptionTypeServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<SubscriptionTypeService>>();

            _service = new SubscriptionTypeService(
                _context,
                logger.Object);
        }


        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDTOIsValid()
        {
            // Arrange
            var dto = CreateSubscriptionTypeDTO();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);

            var saved = await _context.SubscriptionTypes
                .FindAsync(result.Data);


            Assert.NotNull(saved);
            Assert.Equal(dto.NameEn, saved.NameEn);
            Assert.Equal(dto.NameAr, saved.NameAr);
            Assert.Equal(dto.Price, saved.Price);
            Assert.Equal(dto.DaysPerWeek, saved.DaysPerWeek);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidDaysPerWeek_WhenDaysMoreThan7()
        {
            var dto = CreateSubscriptionTypeDTO();
            dto.DaysPerWeek = 8;

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDaysPerWeek, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidDaysPerWeek_WhenDaysLessThan1()
        {
            var dto = CreateSubscriptionTypeDTO();
            dto.DaysPerWeek = 0;

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDaysPerWeek, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidDuration_WhenDurationIsZero()
        {
            var dto = CreateSubscriptionTypeDTO();
            dto.DurationDays = 0;

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDuration, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPrice_WhenPriceIsNegative()
        {
            var dto = CreateSubscriptionTypeDTO();
            dto.Price = -1;

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidPrice, result.Code);
        }

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSubscriptionTypes()
        {
            // Arrange
            await CreateSubscriptionTypes();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSubscriptionType_WhenExists()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            // Act
            var result = await _service.GetByIdAsync(entity.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            Assert.Equal(entity.NameEn, result.Data.NameEn);
            Assert.Equal(entity.Price, result.Data.Price);

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);

        }

        #endregion

        #region ========================= Update =========================

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenSubscriptionTypeExists()
        {
            // Arrange
            var entity = await CreateSubscriptionType();
            var dto = CreateSubscriptionTypeDTO();

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated = await _context.SubscriptionTypes
                .FindAsync(entity.Id);

            Assert.Equal(dto.NameEn, updated.NameEn);
            Assert.Equal(dto.NameAr, updated.NameAr);
            Assert.Equal(dto.Price, updated.Price);

            Assert.NotNull(updated.UpdatedAt);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var dto = CreateSubscriptionTypeDTO();

            // Act
            var result = await _service.UpdateAsync(999, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);

            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidDaysPerWeek_WhenDaysMoreThan7()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            var dto = CreateSubscriptionTypeDTO();
            dto.DaysPerWeek = 8;

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDaysPerWeek, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidDaysPerWeek_WhenDaysLessThan1()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            var dto = CreateSubscriptionTypeDTO();
            dto.DaysPerWeek = 0;

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDaysPerWeek, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidDuration_WhenDurationIsZero()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            var dto = CreateSubscriptionTypeDTO();
            dto.DurationDays = 0;

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidDuration, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidPrice_WhenPriceIsNegative()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            var dto = CreateSubscriptionTypeDTO();
            dto.Price = -10;

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidPrice, result.Code);
        }

        #endregion

        #region ========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_ShouldSoftDelete_WhenSubscriptionTypeExists()
        {
            // Arrange
            var entity = await CreateSubscriptionType();

            // Act
            var result = await _service.DeleteAsync(entity.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var deleted = await _context.SubscriptionTypes
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == entity.Id);


            Assert.NotNull(deleted);
            Assert.True(deleted.IsDeleted);

            Assert.NotNull(deleted.DeletedAt);
            Assert.NotNull(deleted.UpdatedAt);

        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);

            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);

        }

        #endregion

        #region ========================= Helpers =========================
        private async Task<SubscriptionType> CreateSubscriptionType()
        {
            var entity = new SubscriptionType
            {
                NameEn = "Monthly",
                NameAr = "شهري",
                DaysPerWeek = 5,
                DurationDays = 30,
                Price = 100,
                IsActive = true
            };


            _context.SubscriptionTypes.Add(entity);

            await _context.SaveChangesAsync();


            return entity;
        }

        private async Task CreateSubscriptionTypes()
        {
            _context.SubscriptionTypes.AddRange(
                new SubscriptionType
                {
                    NameEn = "Monthly",
                    NameAr = "شهري",
                    DaysPerWeek = 5,
                    DurationDays = 30,
                    Price = 100
                },

                new SubscriptionType
                {
                    NameEn = "Yearly",
                    NameAr = "سنوي",
                    DaysPerWeek = 6,
                    DurationDays = 365,
                    Price = 1000
                }
            );


            await _context.SaveChangesAsync();
        }

        private SubscriptionTypeDTO CreateSubscriptionTypeDTO()
        {
            return new SubscriptionTypeDTO
            {
                NameEn = "Monthly",
                NameAr = "شهري",
                DaysPerWeek = 5,
                DurationDays = 30,
                Price = 100,
                IsActive = true
            };
        }

        #endregion

    }
}
