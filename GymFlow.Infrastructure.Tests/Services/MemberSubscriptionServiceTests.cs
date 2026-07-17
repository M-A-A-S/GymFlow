using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.MemberSubscription;
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
    public class MemberSubscriptionServiceTests
    {
        private readonly TestDbContext _context;
        private readonly MemberSubscriptionService _service;

        public MemberSubscriptionServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;


            _context = new TestDbContext(options);


            var logger = new Mock<ILogger<MemberSubscriptionService>>();


            _service = new MemberSubscriptionService(
                _context,
                logger.Object);
        }

        #region ========================= Add =========================
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDTOIsValid()
        {
            // Arrange
            var dto = CreateMemberSubscriptionDTO();


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);


            var saved = await _context.MemberSubscriptions
                .FindAsync(result.Data);


            Assert.NotNull(saved);

            Assert.Equal(dto.MemberId, saved.MemberId);
            Assert.Equal(dto.SubscriptionTypeId, saved.SubscriptionTypeId);
            Assert.Equal(dto.Price, saved.Price);
            Assert.Equal(dto.Status, saved.Status);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidPrice_WhenPriceIsNegative()
        {
            // Arrange
            var dto = CreateMemberSubscriptionDTO();
            dto.Price = -10;


            // Act
            var result = await _service.AddAsync(dto);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidPrice, result.Code);
            Assert.Equal(400, result.StatusCode);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnInvalidData_WhenDTOIsNull()
        {
            // Act
            var result = await _service.AddAsync(null);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidData, result.Code);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnSubscriptionOverlap_WhenMemberHasExistingSubscription()
        {
            // Arrange
            await CreateMemberSubscription();

            var dto = CreateMemberSubscriptionDTO();

            // Same member and overlapping dates
            dto.MemberId = 1;
            dto.StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
            dto.EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2));

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(ResultCodes.SubscriptionOverlap, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        #endregion

        #region ========================= Get =========================
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMemberSubscriptions()
        {
            // Arrange
            await CreateMemberSubscriptions();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMemberSubscription_WhenExists()
        {
            // Arrange
            var entity = await CreateMemberSubscription();

            // Act
            var result = await _service.GetByIdAsync(entity.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);


            Assert.Equal(entity.MemberId, result.Data.MemberId);
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
        public async Task UpdateAsync_ShouldReturnSuccess_WhenEntityExists()
        {
            // Arrange
            var entity = await CreateMemberSubscription();

            var dto = CreateMemberSubscriptionDTO();

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated = await _context.MemberSubscriptions
                .FindAsync(entity.Id);

            Assert.Equal(dto.MemberId, updated.MemberId);
            Assert.Equal(dto.SubscriptionTypeId, updated.SubscriptionTypeId);
            Assert.Equal(dto.Price, updated.Price);

            Assert.NotNull(updated.UpdatedAt);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var dto = CreateMemberSubscriptionDTO();

            // Act
            var result = await _service.UpdateAsync(999, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);

            Assert.Equal(ResultCodes.NotFound, result.Code);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnInvalidPrice_WhenPriceIsNegative()
        {
            // Arrange
            var entity = await CreateMemberSubscription();

            var dto = CreateMemberSubscriptionDTO();

            dto.Price = -1;

            // Act
            var result = await _service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultCodes.InvalidPrice, result.Code);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSubscriptionOverlap_WhenUpdatingToExistingPeriod()
        {
            // Arrange

            // Existing subscription for member 1
            var first = await CreateMemberSubscription();

            // Another subscription that will conflict
            var second = new MemberSubscription
            {
                MemberId = 1,
                SubscriptionTypeId = 2,

                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),

                Price = 200,
                Status = SubscriptionStatus.Active,
                IsDeleted = false
            };

            _context.MemberSubscriptions.Add(second);

            await _context.SaveChangesAsync();

            var dto = CreateMemberSubscriptionDTO();

            dto.MemberId = 1;

            // Make first subscription overlap second subscription
            dto.StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2));
            dto.EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(4));

            // Act
            var result = await _service.UpdateAsync(first.Id, dto);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal(ResultCodes.SubscriptionOverlap, result.Code);
            Assert.Equal(400, result.StatusCode);
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldSoftDelete_WhenEntityExists()
        {
            // Arrange
            var entity = await CreateMemberSubscription();

            // Act
            var result = await _service.DeleteAsync(entity.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var deleted = await _context.MemberSubscriptions
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
        private async Task<MemberSubscription> CreateMemberSubscription()
        {
            var entity = new MemberSubscription
            {
                MemberId = 1,
                SubscriptionTypeId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                Price = 100,
                Status = SubscriptionStatus.Active,
                IsDeleted = false
            };

            _context.MemberSubscriptions.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        private async Task CreateMemberSubscriptions()
        {
            _context.MemberSubscriptions.AddRange(

                new MemberSubscription
                {
                    MemberId = 1,
                    SubscriptionTypeId = 1,

                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),

                    Price = 100,

                    Status = SubscriptionStatus.Active
                },


                new MemberSubscription
                {
                    MemberId = 2,
                    SubscriptionTypeId = 2,

                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),

                    Price = 300,

                    Status = SubscriptionStatus.Active
                }

            );


            await _context.SaveChangesAsync();

        }

        private MemberSubscriptionDTO CreateMemberSubscriptionDTO()
        {
            return new MemberSubscriptionDTO
            {
                MemberId = 1,

                SubscriptionTypeId = 1,

                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),

                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),

                Price = 100,

                Status = SubscriptionStatus.Active
            };
        }

        #endregion

    }
}
