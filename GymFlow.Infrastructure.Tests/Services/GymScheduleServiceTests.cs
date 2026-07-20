using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.GymSchedule;
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
    public class GymScheduleServiceTests
    {
        #region ========================= Fields & Properties =========================

        private readonly TestDbContext _context;
        private readonly GymScheduleService _service;

        #endregion

        #region ========================= Constructors =========================

        public GymScheduleServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<GymScheduleService>>();

            _service = new GymScheduleService(
                _context,
                logger.Object);
        }

        #endregion

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDtoIsValid()
        {
            var dto = CreateGymScheduleDTO();

            var result = await _service.AddAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(ResultCodes.CreatedSuccessfully, result.Code);

            var entity = await _context.GymSchedules.FindAsync(result.Data);

            Assert.NotNull(entity);
            Assert.Equal(dto.Day, entity.Day);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDtoIsNull()
        {
            var result = await _service.AddAsync(null);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidScheduleTime_WhenStartIsAfterEnd()
        {
            var dto = CreateGymScheduleDTO();

            dto.StartTime = new TimeSpan(15, 0, 0);
            dto.EndTime = new TimeSpan(10, 0, 0);

            var result = await _service.AddAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidScheduleTime, result.Code);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnOverlap_WhenScheduleConflicts()
        {
            await CreateGymScheduleEntity(
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));


            var dto = CreateGymScheduleDTO();

            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(10, 0, 0);
            dto.EndTime = new TimeSpan(12, 0, 0);


            var result = await _service.AddAsync(dto);


            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.GymScheduleOverlap, result.Code);
        }

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSchedules()
        {
            await CreateGymScheduleEntity();

            await CreateGymScheduleEntity(
                DayOfWeek.Tuesday);

            var result = await _service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSchedule_WhenExists()
        {
            var schedule = await CreateGymScheduleEntity();

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
            var schedule = await CreateGymScheduleEntity();

            var dto = CreateGymScheduleDTO();

            dto.Day = DayOfWeek.Friday;
            dto.StartTime = new TimeSpan(14, 0, 0);
            dto.EndTime = new TimeSpan(16, 0, 0);

            var result = await _service.UpdateAsync(schedule.Id, dto);

            Assert.True(result.IsSuccess);

            var updated = await _context.GymSchedules.FindAsync(schedule.Id);


            Assert.Equal(DayOfWeek.Friday, updated.Day);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            var dto = CreateGymScheduleDTO();


            var result = await _service.UpdateAsync(999, dto);


            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.NotFound, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidScheduleTime_WhenStartIsAfterEnd()
        {
            var schedule = await CreateGymScheduleEntity();

            var dto = CreateGymScheduleDTO();

            dto.StartTime = new TimeSpan(15, 0, 0);
            dto.EndTime = new TimeSpan(10, 0, 0);

            var result = await _service.UpdateAsync(schedule.Id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidScheduleTime, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOverlap_WhenScheduleOverlaps()
        {
            var schedule1 = await CreateGymScheduleEntity(
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));

            var schedule2 = await CreateGymScheduleEntity(
                DayOfWeek.Monday,
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0));

            var dto = CreateGymScheduleDTO();

            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(10, 0, 0);
            dto.EndTime = new TimeSpan(13, 0, 0);

            var result = await _service.UpdateAsync(schedule2.Id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.GymScheduleOverlap, result.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotReturnOverlap_WhenUpdatingSameSchedule()
        {
            var schedule = await CreateGymScheduleEntity(
                DayOfWeek.Monday,
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 0, 0));

            var dto = CreateGymScheduleDTO();

            dto.Day = DayOfWeek.Monday;
            dto.StartTime = new TimeSpan(9, 0, 0);
            dto.EndTime = new TimeSpan(11, 0, 0);

            var result = await _service.UpdateAsync(schedule.Id, dto);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDelete_WhenExists()
        {
            var schedule = await CreateGymScheduleEntity();

            var result = await _service.DeleteAsync(schedule.Id);

            Assert.True(result.IsSuccess);

            var deleted = await _context.GymSchedules.FindAsync(schedule.Id);

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
        private async Task<GymSchedule> CreateGymScheduleEntity(
        DayOfWeek day = DayOfWeek.Monday,
        TimeSpan? start = null,
        TimeSpan? end = null)
        {
            var schedule = new GymSchedule
            {
                Day = day,
                StartTime = start ?? new TimeSpan(9, 0, 0),
                EndTime = end ?? new TimeSpan(11, 0, 0),
                Gender = Gender.Male
            };


            _context.GymSchedules.Add(schedule);

            await _context.SaveChangesAsync();

            return schedule;
        }

        private GymScheduleDTO CreateGymScheduleDTO()
        {
            return new GymScheduleDTO
            {
                Day = DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(11, 0, 0),
                Gender = Gender.Male
            };
        }

        #endregion

    }
}
