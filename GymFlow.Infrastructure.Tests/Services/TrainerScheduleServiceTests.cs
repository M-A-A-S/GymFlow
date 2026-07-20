using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.TrainerSchedule;
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
    public class TrainerScheduleServiceTests
    {
        #region ========================= Fields & Properties =========================
        private readonly TestDbContext _context;
        private readonly TrainerScheduleService _service;
        #endregion

        #region ========================= Constructors =========================
        public TrainerScheduleServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<TrainerScheduleService>>();

            _service = new TrainerScheduleService(
                _context,
                logger.Object);
        }
        #endregion

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDtoIsValid()
        {
            var trainer = await CreateTrainerEntity();

            var dto = CreateTrainerScheduleDTO(trainer.Id);

            var result = await _service.AddAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);

            var entity = await _context.TrainerSchedules.FindAsync(result.Data);

            Assert.NotNull(entity);
            Assert.Equal(dto.Day, entity.Day);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnTrainerNotFound_WhenTrainerDoesNotExist()
        {
            var dto = CreateTrainerScheduleDTO(999);

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.TrainerNotFound, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidScheduleTime_WhenStartIsAfterEnd()
        {
            var trainer = await CreateTrainerEntity();

            var dto = CreateTrainerScheduleDTO(trainer.Id);
            dto.StartTime = new TimeSpan(15, 0, 0);
            dto.EndTime = new TimeSpan(10, 0, 0);

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidScheduleTime, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnOverlap_WhenScheduleConflicts()
        {
            var trainer = await CreateTrainerEntity();

            await CreateTrainerScheduleEntity(
                trainer.Id,
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));

            var dto = CreateTrainerScheduleDTO(trainer.Id);

            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(10, 0, 0);
            dto.EndTime = new TimeSpan(12, 0, 0);

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.TrainerScheduleOverlap, result.Code);
        }

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSchedules()
        {
            var trainer = await CreateTrainerEntity();

            await CreateTrainerScheduleEntity(trainer.Id);
            await CreateTrainerScheduleEntity(trainer.Id, DayOfWeek.Tuesday);

            var result = await _service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSchedule_WhenExists()
        {
            var trainer = await CreateTrainerEntity();

            var schedule = await CreateTrainerScheduleEntity(trainer.Id);

            var result = await _service.GetByIdAsync(schedule.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule.Day, result.Data.Day);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenMissing()
        {
            var result = await _service.GetByIdAsync(999);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        #endregion

        #region ========================= Update =========================
        [Fact]
        public async Task UpdateAsync_ShouldUpdateSchedule_WhenValid()
        {
            var trainer = await CreateTrainerEntity();

            var schedule = await CreateTrainerScheduleEntity(trainer.Id);

            var dto = CreateTrainerScheduleDTO(trainer.Id);

            dto.Day = DayOfWeek.Friday;
            dto.StartTime = new TimeSpan(14, 0, 0);
            dto.EndTime = new TimeSpan(16, 0, 0);

            var result = await _service.UpdateAsync(schedule.Id, dto);

            Assert.True(result.IsSuccess);

            var updated = await _context.TrainerSchedules.FindAsync(schedule.Id);

            Assert.Equal(DayOfWeek.Friday, updated.Day);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            var trainer = await CreateTrainerEntity();

            var dto = CreateTrainerScheduleDTO(trainer.Id);

            var result = await _service.UpdateAsync(999, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrainerNotFound_WhenTrainerDoesNotExist()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();
            var schedule = await CreateTrainerScheduleEntity(trainer.Id);

            var dto = CreateTrainerScheduleDTO(999);

            // Act
            var result = await _service.UpdateAsync(schedule.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.TrainerNotFound, result.Code);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidScheduleTime_WhenStartTimeIsAfterEndTime()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();
            var schedule = await CreateTrainerScheduleEntity(trainer.Id);

            var dto = CreateTrainerScheduleDTO(trainer.Id);
            dto.StartTime = new TimeSpan(15, 0, 0);
            dto.EndTime = new TimeSpan(10, 0, 0);

            // Act
            var result = await _service.UpdateAsync(schedule.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidScheduleTime, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrainerScheduleOverlap_WhenScheduleOverlaps()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();

            var schedule1 = await CreateTrainerScheduleEntity(
                trainer.Id,
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));

            var schedule2 = await CreateTrainerScheduleEntity(
                trainer.Id,
                DayOfWeek.Monday,
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0));

            var dto = CreateTrainerScheduleDTO(trainer.Id);
            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(10, 0, 0);
            dto.EndTime = new TimeSpan(13, 0, 0);

            // Act
            var result = await _service.UpdateAsync(schedule2.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.TrainerScheduleOverlap, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotReturnOverlap_WhenUpdatingSameSchedule()
        {
            // Arrange
            var trainer = await CreateTrainerEntity();

            var schedule = await CreateTrainerScheduleEntity(
                trainer.Id,
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));

            var dto = CreateTrainerScheduleDTO(trainer.Id);
            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(9, 0, 0);
            dto.EndTime = new TimeSpan(11, 0, 0);

            // Act
            var result = await _service.UpdateAsync(schedule.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDelete_WhenExists()
        {
            var trainer = await CreateTrainerEntity();

            var schedule = await CreateTrainerScheduleEntity(trainer.Id);

            var result = await _service.DeleteAsync(schedule.Id);

            Assert.True(result.IsSuccess);

            var deleted = await _context.TrainerSchedules.FindAsync(schedule.Id);

            Assert.True(deleted.IsDeleted);
            Assert.NotNull(deleted.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenMissing()
        {
            var result = await _service.DeleteAsync(999);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        #endregion

        #region ========================= Helpers =========================
        private async Task<Trainer> CreateTrainerEntity()
        {
            var trainer = new Trainer
            {
                FullName = "Mohammed",
                PhoneNumber = "0999888777",
                Salary = 500,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return trainer;
        }

        private async Task<TrainerSchedule> CreateTrainerScheduleEntity(
            int trainerId,
            DayOfWeek day = DayOfWeek.Monday,
            TimeSpan? start = null,
            TimeSpan? end = null)
        {
            var schedule = new TrainerSchedule
            {
                TrainerId = trainerId,
                Day = day,
                StartTime = start ?? new TimeSpan(9, 0, 0),
                EndTime = end ?? new TimeSpan(11, 0, 0)
            };

            _context.TrainerSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return schedule;
        }

        private TrainerScheduleDTO CreateTrainerScheduleDTO(int trainerId)
        {
            return new TrainerScheduleDTO
            {
                TrainerId = trainerId,
                Day = DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(11, 0, 0)
            };
        }

        #endregion

    }
}
