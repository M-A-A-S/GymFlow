using GymFlow.Domain.Constants;
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
    public class MemberAttendanceServiceTests
    {
        #region ========================= Fields & Properties =========================
        private readonly TestDbContext _context;
        private readonly MemberAttendanceService _service;
        #endregion

        #region ========================= Constructors =========================
        public MemberAttendanceServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new TestDbContext(options);


            var logger = new Mock<ILogger<MemberAttendanceService>>();


            _service = new MemberAttendanceService(
                _context,
                logger.Object);
        }
        #endregion

        #region ========================= Get Daily Attendance =========================
        [Fact]
        public async Task GetDailyAttendanceAsync_ShouldReturnActiveMembersWithAttendance()
        {
            // Arrange

            var date = DateOnly.FromDateTime(DateTime.Today);
            var member = await CreateMemberWithSubscription();

            _context.MemberAttendances.Add(
                new MemberAttendance
                {
                    MemberId = member.Id,
                    AttendanceDate = date,
                    CheckIn = new TimeOnly(8, 30)
                });


            await _context.SaveChangesAsync();


            // Act

            var result =
                await _service.GetDailyAttendanceAsync(date);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            var item = result.Data.First();
            Assert.Equal(member.Id, item.MemberId);
            Assert.Equal(member.FullName, item.MemberName);
            Assert.NotNull(item.AttendanceId);
            Assert.Equal(new TimeOnly(8, 30), item.CheckIn);

        }

        [Fact]
        public async Task GetDailyAttendanceAsync_ShouldNotReturnExpiredMembers()
        {
            // Arrange

            var date = DateOnly.FromDateTime(DateTime.Today);

            var member = new Member
            {
                FullName = "Expired Member",
                PhoneNumber = "1234567890",
                Gender = Gender.Male,

                MemberSubscriptions =
                [
                    new MemberSubscription
                {
                    StartDate = date.AddDays(-30),
                    EndDate = date.AddDays(-1)
                }
                ]
            };


            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            // Act

            var result =
                await _service.GetDailyAttendanceAsync(date);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);

        }

        #endregion

        #region ========================= Check In =========================
        [Fact]
        public async Task CheckInAsync_ShouldCreateAttendance_WhenMemberNotCheckedIn()
        {
            // Arrange
            var member = await CreateMemberWithSubscription();

            var date = DateOnly.FromDateTime(DateTime.UtcNow);

            // Act
            var result =
                await _service.CheckInAsync(member.Id, date);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var attendance =
                await _context.MemberAttendances
                .FirstOrDefaultAsync();


            Assert.NotNull(attendance);
            Assert.Equal(member.Id, attendance.MemberId);
            Assert.NotEqual(default, attendance.CheckIn);

        }

        [Fact]
        public async Task CheckInAsync_ShouldReturnAlreadyCheckedIn_WhenAttendanceExists()
        {
            // Arrange
            var member = await CreateMemberWithSubscription();

            _context.MemberAttendances.Add(
                new MemberAttendance
                {
                    MemberId = member.Id,
                    AttendanceDate =
                        DateOnly.FromDateTime(DateTime.Today),

                    CheckIn =
                        new TimeOnly(8, 0)
                });

            await _context.SaveChangesAsync();

            // Act
            var date = DateOnly.FromDateTime(DateTime.Today);

            var result =
                await _service.CheckInAsync(member.Id, date);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.MemberAlreadyCheckedIn,
                result.Code);

        }

        [Fact]
        public async Task CheckInAsync_ShouldCreateAttendance_ForSelectedDate()
        {
            // Arrange

            var member =
                await CreateMemberWithSubscription();

            var selectedDate =
                new DateOnly(2026, 7, 1);


            // Act

            var result =
                await _service.CheckInAsync(
                    member.Id,
                    selectedDate);


            // Assert

            Assert.True(result.IsSuccess);


            var attendance =
                await _context.MemberAttendances
                .FirstAsync();


            Assert.Equal(
                selectedDate,
                attendance.AttendanceDate);
        }

        #endregion

        #region ========================= Check Out =========================
        [Fact]
        public async Task CheckOutAsync_ShouldUpdateCheckout_WhenAttendanceExists()
        {
            // Arrange
            var attendance =
                await CreateAttendance();

            // Act
            var result =
                await _service.CheckOutAsync(attendance.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated =
                await _context.MemberAttendances
                .FindAsync(attendance.Id);

            Assert.NotNull(updated.CheckOut);

        }

        [Fact]
        public async Task CheckOutAsync_ShouldReturnNotFound_WhenAttendanceDoesNotExist()
        {
            // Act

            var result =
                await _service.CheckOutAsync(999);

            // Assert

            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.AttendanceNotFound,
                result.Code);

        }

        #endregion

        #region ========================= Helpers =========================
        private async Task<Member> CreateMemberWithSubscription()
        {
            var member = new Member
            {
                FullName = "Ahmed Ali",
                PhoneNumber = "1234567890",
                Gender = Gender.Male,

                MemberSubscriptions =
                [
                    new MemberSubscription
                {
                    StartDate =
                        DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),

                    EndDate =
                        DateOnly.FromDateTime(DateTime.Today.AddDays(20))
                }
                ]
            };


            _context.Members.Add(member);

            await _context.SaveChangesAsync();


            return member;
        }

        private async Task<MemberAttendance> CreateAttendance()
        {
            var member =
                await CreateMemberWithSubscription();


            var attendance = new MemberAttendance
            {
                MemberId = member.Id,

                AttendanceDate =
                    DateOnly.FromDateTime(DateTime.Today),

                CheckIn =
                    new TimeOnly(8, 0)
            };


            _context.MemberAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        [Fact]
        public async Task CheckOutAsync_ShouldReturnAlreadyCheckedOut_WhenAlreadyCheckedOut()
        {
            // Arrange

            var attendance =
                await CreateAttendance();

            attendance.CheckOut =
                new TimeOnly(17, 0);

            await _context.SaveChangesAsync();

            // Act

            var result =
                await _service.CheckOutAsync(attendance.Id);

            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.AlreadyCheckedOut,
                result.Code);

        }

        #endregion

    }
}
