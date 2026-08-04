using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.File;
using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Services;
using GymFlow.Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services.SystemSetting
{

    public class SystemSettingServiceTests
    {

        #region ========================= Fields =========================

        private readonly TestDbContext _context;
        private readonly SystemSettingService _service;
        private readonly Mock<IFileService> _fileServiceMock;

        #endregion


        #region ========================= Constructor =========================

        public SystemSettingServiceTests()
        {
            var options =
                new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new TestDbContext(options);


            var logger =
                new Mock<ILogger<SystemSettingService>>();


            _fileServiceMock =
                new Mock<IFileService>();

            var cache = new MemoryCache(
new MemoryCacheOptions());


            _service =
                new SystemSettingService(
                    _context,
                    logger.Object,
                    _fileServiceMock.Object,
                    cache);
        }

        #endregion


        #region ========================= Add =========================

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessWithId_WhenDtoIsValid()
        {
            // Arrange

            var dto =
                CreateSystemSettingDTO();


            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.SystemSettingsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "logo.jpg",
                        ResultCodes.FileSaved));


            // Act

            var result =
                await _service.AddAsync(dto);



            // Assert

            Assert.True(result.IsSuccess);

            Assert.Equal(
                ResultCodes.CreatedSuccessfully,
                result.Code);


            Assert.True(
                result.Data > 0);



            var entity =
                await _context.SystemSettings
                .FindAsync(result.Data);



            Assert.NotNull(entity);


            Assert.Equal(
                dto.NameEn,
                entity.NameEn);


            Assert.Equal(
                "logo.jpg",
                entity.LogoUrl);



            _fileServiceMock.Verify(
                x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.SystemSettingsFolder,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenFileSaveFails()
        {
            // Arrange

            var dto =
                CreateSystemSettingDTO();



            _fileServiceMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    Constants.SystemSettingsFolder,
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
        public async Task AddAsync_ShouldReturnInvalidData_WhenDtoIsNull()
        {
            // Act

            var result =
                await _service.AddAsync(null);



            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.InvalidData,
                result.Code);
        }

        #endregion


        #region ========================= Get =========================

        [Fact]
        public async Task GetAllAsync_ShouldReturnSystemSettings()
        {
            // Arrange

            await CreateSystemSettingEntity();

            await CreateSystemSettingEntity(
                "Second Company");



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
        public async Task GetByIdAsync_ShouldReturnSystemSetting_WhenExists()
        {
            // Arrange

            var setting =
                await CreateSystemSettingEntity();



            // Act

            var result =
                await _service.GetByIdAsync(
                    setting.Id);



            // Assert

            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Data);


            Assert.Equal(
                setting.NameEn,
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
                HttpStatusCodes.NotFound,
                result.StatusCode);
        }

        #endregion


        #region ========================= Update =========================

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSystemSetting_WhenValid()
        {
            // Arrange

            var setting =
                await CreateSystemSettingEntity();



            var dto =
                CreateSystemSettingDTO(
                    "Updated Company");



            _fileServiceMock
                .Setup(x => x.ReplaceAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    Constants.SystemSettingsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<string?>.Success(
                        "new-logo.jpg"));



            // Act

            var result =
                await _service.UpdateAsync(
                    setting.Id,
                    dto);



            // Assert

            Assert.True(result.IsSuccess);



            var updated =
                await _context.SystemSettings
                .FindAsync(setting.Id);



            Assert.Equal(
                "Updated Company",
                updated.NameEn);


            Assert.Equal(
                "new-logo.jpg",
                updated.LogoUrl);


            Assert.NotNull(
                updated.UpdatedAt);
        }


        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenSystemSettingMissing()
        {
            // Arrange

            var dto =
                CreateSystemSettingDTO();



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
        public async Task DeleteAsync_ShouldSoftDeleteSystemSetting_WhenExists()
        {
            // Arrange

            var setting =
                await CreateSystemSettingEntity();



            _fileServiceMock
                .Setup(x => x.DeleteAsync(
                    It.IsAny<string>(),
                    Constants.SystemSettingsFolder,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<bool>.Success(true));



            // Act

            var result =
                await _service.DeleteAsync(
                    setting.Id);



            // Assert

            Assert.True(result.IsSuccess);



            var deleted =
                await _context.SystemSettings
                .FindAsync(setting.Id);



            Assert.True(
                deleted.IsDeleted);


            Assert.NotNull(
                deleted.DeletedAt);


            Assert.NotNull(
                deleted.UpdatedAt);



            _fileServiceMock.Verify(
                x => x.DeleteAsync(
                    setting.LogoUrl,
                    Constants.SystemSettingsFolder,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenSystemSettingMissing()
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

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenDeleteFileFails()
        {
            // Arrange

            var setting =
                await CreateSystemSettingEntity();



            _fileServiceMock
                .Setup(x => x.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<bool>.Failure(
                        ResultCodes.FileDeleteFailed,
                        500));



            // Act

            var result =
                await _service.DeleteAsync(
                    setting.Id);



            // Assert

            Assert.False(result.IsSuccess);

            Assert.Equal(
                ResultCodes.FileDeleteFailed,
                result.Code);
        }

        #endregion


        #region ========================= Helpers =========================

        private async Task<Domain.Entities.SystemSetting> CreateSystemSettingEntity(
            string name = "My Company")
        {
            var entity =
                new Domain.Entities.SystemSetting
                {
                    NameEn = name,
                    NameAr = "الشركة",
                    AddressEn = "Khartoum",
                    AddressAr = "الخرطوم",
                    Phone = "0999999999",
                    Email = "test@test.com",
                    Website = "www.test.com",
                    Facebook = "facebook",
                    Instagram = "instagram",
                    TaxNumber = "123456",
                    Currency = "SDG",
                    LogoUrl = "logo.jpg",
                    ReceiptFooterEn = "Thank you",
                    ReceiptFooterAr = "شكرا"
                };


            _context.SystemSettings.Add(entity);

            await _context.SaveChangesAsync();


            return entity;
        }



        private SystemSettingDTO CreateSystemSettingDTO(
            string name = "My Company")
        {
            return new SystemSettingDTO
            {
                NameEn = name,
                NameAr = "الشركة",
                AddressEn = "Khartoum",
                AddressAr = "الخرطوم",
                Phone = "0999999999",
                Email = "test@test.com",
                Website = "www.test.com",
                Facebook = "facebook",
                Instagram = "instagram",
                TaxNumber = "123456",
                Currency = "SDG",
                LogoUrl = null,
                ReceiptFooterEn = "Thank you",
                ReceiptFooterAr = "شكرا"
            };
        }

        #endregion
    }

}
