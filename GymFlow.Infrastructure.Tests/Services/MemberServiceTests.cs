using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services
{
    public class MemberServiceTests
    {
        private readonly TestDbContext _context;
        private readonly MemberService _service;

        public MemberServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            var logger = new Mock<ILogger<MemberService>>();

            _service = new MemberService(_context, logger.Object);
        }


        #region Add
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenOnlyDTORequiredFields()
        {
            // Arrange
            var dto = CreateMemberDTOWithOnlyRequiredFields();

            // Act
            var result = await _service.AddAsync(dto);
            

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);

            var savedMember = await _context.Members.FindAsync(result.Data);
            var membersCount = await _context.Members.CountAsync();

            Assert.Equal(1, membersCount);
            Assert.NotNull(savedMember);
            Assert.Equal(savedMember.PhoneNumber, savedMember.PhoneNumber);
            Assert.Equal(savedMember.FullName, savedMember.FullName);
            Assert.Equal(savedMember.Gender, savedMember.Gender);

        }
        
        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDTOIsValidAndUnique()
        {
            // Arrange
            var dto = CreateMemberDTO();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert

            var savedMember = await _context.Members.FindAsync(result.Data);
            var membersCount = await _context.Members.CountAsync();

            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);

            Assert.Equal(1, membersCount);
            Assert.NotNull(savedMember);
            Assert.Equal(savedMember.PhoneNumber, savedMember.PhoneNumber);
            Assert.Equal(savedMember.FullName, savedMember.FullName);
            Assert.Equal(savedMember.Gender, savedMember.Gender);
            Assert.Equal(savedMember.Email, savedMember.Email);
            Assert.Equal(savedMember.Address, savedMember.Address);
            Assert.Equal(savedMember.BirthDate, savedMember.BirthDate);

        }

        [Fact]
        public async Task AddAsync_ShouldIgnoreExistingNullEmails_WhenDTOContainsNullEmail()
        {
            // Arrange
            await CreateMemberAsync(email: null, phone: "111111111");

            var dto = CreateMemberDTO(
                email: null,
                phone: "222222222");

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data > 0);
            Assert.Equal(result.StatusCode, 200);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnEmailExists_WhenEmailAlreadyExists()
        {
            // Arrange
            string duplicateEmail = "duplicate@example.com";
            await CreateMemberAsync(email: duplicateEmail);

            var dto = CreateMemberDTO(email: duplicateEmail);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Code, ResultCodes.EmailExists);
            Assert.Equal(result.StatusCode, 409);

        }

        [Fact]
        public async Task AddAsync_ShouldReturnPhoneExists_WhenPhoneAlreadyExists()
        {
            // Arrange
            string duplicatePhone = "123456789";
            await CreateMemberAsync(phone: duplicatePhone);

            var dto = CreateMemberDTO(phone: duplicatePhone, email: "1@1.com");

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Code, ResultCodes.PhoneExists);
            Assert.Equal(result.StatusCode, 409);

        }
        #endregion

        #region Get
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMembers()
        {
            // Arrange
            await CreateMembersList();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(result.Data.Count(), 2);


        }
        
        [Fact]
        public async Task GetByIdAsync_ShouldReturnMember_WhenFound()
        {
            // Arrange
            var member = await CreateMemberAsync();

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(result.Data.Email, member.Email);

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.Code, ResultCodes.NotFound);
            Assert.Equal(result.StatusCode, 404);

        }
        
        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUpdateIsValid()
        {
            // Arrange
            var existingMember = await CreateMemberAsync(email: "original@example.com");

            var updatedDTO = CreateMemberDTO(email: "updated@example.com");

            // Act
            var result = await _service.UpdateAsync(1, updatedDTO);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated = await _context.Members.FindAsync(1);

            Assert.Equal(updated.Email, updatedDTO.Email);
            Assert.Equal(updated.FullName, updatedDTO.FullName);
            Assert.Equal(updated.Address, updatedDTO.Address);
            Assert.Equal(updated.PhoneNumber, updatedDTO.PhoneNumber);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenMemberDoesNotExist()
        {
            // Arrange
            var updatedDTO = CreateMemberDTO(email: "updated@example.com");

            // Act
            var result = await _service.UpdateAsync(999, updatedDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal(result.StatusCode, 404);

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnEmailExists_WhenUpdatedEmailBelongsToAnotherMember()
        {
            // Arrange
            await CreateMembersList();

            var updatedDTO = CreateMemberDTO(email: "test2@test.com");

            // Act
            var result = await _service.UpdateAsync(1, updatedDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal(result.StatusCode, 409);
            Assert.Equal(result.Code, ResultCodes.EmailExists);

        }

        [Fact]
        public async Task UpdateAsync_ShouldAllowNullEmail()
        {
            // Arrange
            await CreateMemberAsync(
                email: "existing@test.com",
                phone: "111111111");

            var member = await CreateMemberAsync(
                email: null,
                phone: "222222222");

            var dto = CreateMemberDTO(
                email: null,
                phone: "222222222");

            // Act
            var result = await _service.UpdateAsync(member.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnPhoneExists_WhenUpdatedPhoneBelongsToAnotherMember()
        {
            // Arrange
            await CreateMembersList();

            var updatedDTO = CreateMemberDTO(phone: "222222222");

            // Act
            var result = await _service.UpdateAsync(1, updatedDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal(result.StatusCode, 409);
            Assert.Equal(result.Code, ResultCodes.PhoneExists);

        }

        [Fact]
        public async Task UpdateAsync_ShouldAllowKeepingSameEmailAndPhone()
        {
            // Arrange
            var member = await CreateMemberAsync(
                email: "same@test.com",
                phone: "111111111");

            var dto = CreateMemberDTO(
                email: "same@test.com",
                phone: "111111111");

            // Act
            var result = await _service.UpdateAsync(member.Id, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            var updated = await _context.Members.FindAsync(member.Id);
            Assert.Equal(dto.FullName, updated.FullName);
            Assert.Equal(dto.Email, updated.Email);
            Assert.Equal(dto.PhoneNumber, updated.PhoneNumber);
        }

        #endregion

        #region Delete
        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteMember_WhenIdExists()
        {
            // Arrange
            await CreateMemberAsync();


            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            // this will pass because the query filter only work with linq queries, so we need to use FirstOrDefaultAsync instead of FindAsync
            //var deleted = await _context.Members.FindAsync(1); 
            var deleted = await _context.Members
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == 1);

            Assert.True(deleted.IsDeleted);
            Assert.NotNull(deleted.DeletedAt);

        }
        
        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal(result.StatusCode, 404);

        }
        #endregion


        #region Private Helper Methods
        private async Task<Member> CreateMemberAsync(
            string email = "test@test.com",
            string phone = "123456789")
        {
            var member = new Member
            {
                FullName = "Test Member",
                Email = email,
                PhoneNumber = phone,
                Address = "Sudan",
                BirthDate = new DateOnly(2001, 10, 4),
                Status = MemberStatus.Active,
                RegisterDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Gender = Gender.Male
            };

            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return member;  
        }


        private async Task CreateMembersList()
        {
            _context.Members.AddRange(new List<Member>
            {
                new Member
                {
                    FullName = "Test Member 1",
                    Email = "test1@test.com",
                    PhoneNumber = "111111111",
                    Address = "Sudan",
                    BirthDate = new DateOnly(2001, 10, 4),
                    Status = MemberStatus.Active,
                    RegisterDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Gender = Gender.Male
                },
                new Member
                {
                    FullName = "Test Member 2",
                    Email = "test2@test.com",
                    PhoneNumber = "222222222",
                    Address = "Sudan",
                    BirthDate = new DateOnly(2001, 10, 4),
                    Status = MemberStatus.Active,
                    RegisterDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Gender = Gender.Male
                },
            });

            await _context.SaveChangesAsync();
        }

        private MemberDTO CreateMemberDTO(
            string email = "test@test.com",
            string phone = "123456789")
        {
            return  new MemberDTO
            {
                FullName = "Test Member",
                Email = email,
                PhoneNumber = phone,
                Address = "Sudan",
                BirthDate = new DateOnly(2001, 10, 4),
                Status = MemberStatus.Active,
                RegisterDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Gender = Gender.Male
            };
        }

        private MemberDTO CreateMemberDTOWithOnlyRequiredFields(
            string phone = "123456789")
        {
            return new MemberDTO
            {
                FullName = "Test Member",
                PhoneNumber = phone,
                Gender = Gender.Male
            };
        }

        #endregion

    }

}

