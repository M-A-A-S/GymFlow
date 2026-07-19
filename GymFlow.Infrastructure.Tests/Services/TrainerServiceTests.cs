using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Trainer;
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
    public class TrainerServiceTests
    {

        #region ========================= Fields & Properties =========================
        private readonly TestDbContext _context;
        private readonly TrainerService _service;
        #endregion

        #region ========================= Constructors =========================
        public TrainerServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<TrainerService>>();

            _service = new TrainerService(
                _context,
                logger.Object);
        }
        #endregion


        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenDtoIsValid()
        {
            // Arrange
            var dto = CreateTrainerDTO();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);
            Assert.True(result.Data > 0);

            var entityInDb = await _context.Trainers.FindAsync(result.Data);
            Assert.NotNull(entityInDb);
            Assert.Equal(dto.FullName, entityInDb.FullName);
            Assert.Equal(dto.PhoneNumber, entityInDb.PhoneNumber);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNegativeValue_WhenSalaryIsNegative()
        {
            // Arrange
            var dto = CreateTrainerDTO();
            dto.Salary = -1000m;

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.ValueCannotBeNegative, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPhoneExists_WhenPhoneNumberAlreadyRegistered()
        {
            // Arrange
            var existingTrainer = await CreateTrainerEntity(phone: "111222333");
            var duplicateDto = CreateTrainerDTO(phone: "111222333");

            // Act
            var result = await _service.AddAsync(duplicateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.PhoneExists, result.Code);
            Assert.Equal(409, result.StatusCode);
        }

        #endregion


        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTrainers()
        {
            // Arrange
            await CreateTrainerEntity(phone: "123");
            await CreateTrainerEntity(phone: "456");

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTrainer_WhenIdExists()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();

            // Act
            var result = await _service.GetByIdAsync(trainer.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(trainer.FullName, result.Data.FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnFilteredResults_WhenQueryMatches()
        {
            // Arrange
            await CreateTrainerEntity(name: "Captain America", phone: "11111");
            await CreateTrainerEntity(name: "Iron Man", phone: "22222");

            // Act
            var result = await _service.SearchAsync("Captain");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("Captain America", result.Data.First().FullName);
        }

        #endregion


        #region ========================= Update =========================
        [Fact]
        public async Task UpdateAsync_ShouldUpdateFields_WhenValid()
        {
            // Arrange
            var trainer = await CreateTrainerEntity(name: "Old Name");
            var dto = CreateTrainerDTO(name: "New Name");
            dto.Salary = 600m;

            // Act
            var result = await _service.UpdateAsync(trainer.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updatedEntity = await _context.Trainers.FindAsync(trainer.Id);
            Assert.Equal("New Name", updatedEntity.FullName);
            Assert.Equal(600.00m, updatedEntity.Salary);
            Assert.NotNull(updatedEntity.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenTrainerDoesNotExist()
        {
            // Arrange
            var dto = CreateTrainerDTO();

            // Act
            var result = await _service.UpdateAsync(999, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion


        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDelete_WhenTrainerExists()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();

            // Act
            var result = await _service.DeleteAsync(trainer.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var deletedEntity = await _context.Trainers.FindAsync(trainer.Id);
            Assert.True(deletedEntity.IsDeleted);
            Assert.NotNull(deletedEntity.DeletedAt);
            Assert.NotNull(deletedEntity.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenTrainerDoesNotExist()
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
        private async Task<Trainer> CreateTrainerEntity(string name = "Mohammed Alfatih", string phone = "0999888777")
        {
            var trainer = new Trainer
            {
                FullName = name,
                PhoneNumber = phone,
                Salary = 450.00m,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return trainer;
        }

        private TrainerDTO CreateTrainerDTO(string name = "Mohammed Alfatih", string phone = "0999888777")
        {
            return new TrainerDTO
            {
                FullName = name,
                PhoneNumber = phone,
                Salary = 450.00m,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
            };
        }

        #endregion
    }
}
