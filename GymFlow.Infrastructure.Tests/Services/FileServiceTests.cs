using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.File;
using GymFlow.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Services
{
    public class FileServiceTests
    {
        #region ========================= Fields & Properties =========================
        private readonly string _testFolder;
        private readonly FileService _fileService;

        #endregion

        #region ========================= Constructors =========================
        public FileServiceTests()
        {
            _testFolder =
            Path.Combine(
                Path.GetTempPath(),
                "FileServiceTests",
                Guid.NewGuid().ToString());


            Directory.CreateDirectory(_testFolder);

            var options = new FileValidationOptions
            {
                MaxFileSizeBytes = 5 * 1024 * 1024,

                AllowedExtensions =
            [
                ".jpg",
                ".jpeg",
                ".png"
            ]
            };

            var logger =
            new Mock<ILogger<FileService>>();



            _fileService =
                new FileService(
                    _testFolder,
                    options,
                    logger.Object);
        }

        #endregion

        #region ========================= Save =========================
        [Fact]
        public async Task SaveAsync_ShouldSaveFile_WhenFileIsValid()
        {
            // Arrange
            var request =
                CreateFile(
                    "test.jpg",
                    "hello image");


            // Act
            var result =
                await _fileService.SaveAsync(
                    request,
                    null,
                    "Products");


            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);


            var savedFile =
                Path.Combine(
                    _testFolder,
                    "Products",
                    result.Data!);


            Assert.True(File.Exists(savedFile));
        }

        [Fact]
        public async Task SaveAsync_ShouldReturnUrl_WhenFileIsNull()
        {
            // Act
            var result =
                await _fileService.SaveAsync(
                    null,
                    "https://example.com/image.jpg",
                    "Products");


            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(
                "https://example.com/image.jpg",
                result.Data);
        }

        [Fact]
        public async Task SaveAsync_ShouldFail_WhenExtensionIsInvalid()
        {
            // Arrange
            var request =
                CreateFile(
                    "test.exe",
                    "invalid");


            // Act
            var result =
                await _fileService.SaveAsync(
                    request,
                    null,
                    "Products");


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(
                ResultCodes.FileInvalidExtension,
                result.Code);
        }

        #endregion

        #region ========================= Repleace =========================
        [Fact]
        public async Task ReplaceAsync_ShouldDeleteOldFileAndSaveNew_File()
        {
            // Arrange

            var oldFile =
                CreateFile(
                    "old.jpg",
                    "old");


            var oldResult =
                await _fileService.SaveAsync(
                    oldFile,
                    null,
                    "Products");


            var newFile =
                CreateFile(
                    "new.jpg",
                    "new");



            // Act

            var result =
                await _fileService.ReplaceAsync(
                    newFile,
                    null,
                    oldResult.Data,
                    "Products");



            // Assert

            Assert.True(result.IsSuccess);


            var oldPath =
                Path.Combine(
                    _testFolder,
                    "Products",
                    oldResult.Data!);


            Assert.False(File.Exists(oldPath));


            var newPath =
                Path.Combine(
                    _testFolder,
                    "Products",
                    result.Data!);


            Assert.True(File.Exists(newPath));
        }

        #endregion

        #region ========================= Delete =========================
        [Fact]
        public async Task DeleteAsync_ShouldDeleteFile()
        {
            // Arrange

            var file =
                CreateFile(
                    "delete.jpg",
                    "delete");


            var save =
                await _fileService.SaveAsync(
                    file,
                    null,
                    "Products");



            // Act

            var result =
                await _fileService.DeleteAsync(
                    save.Data,
                    "Products");



            // Assert

            Assert.True(result.IsSuccess);


            var path =
                Path.Combine(
                    _testFolder,
                    "Products",
                    save.Data!);


            Assert.False(File.Exists(path));
        }

        [Fact]
        public async Task DeleteAsync_ShouldIgnoreExternalUrl()
        {
            // Act

            var result =
                await _fileService.DeleteAsync(
                    "https://site.com/image.jpg",
                    "Products");



            // Assert

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region ========================= Helpers =========================
        private static FileUploadRequest CreateFile(
        string fileName,
        string content)
        {
            var bytes =
                System.Text.Encoding.UTF8
                    .GetBytes(content);


            return new FileUploadRequest
            {
                FileName = fileName,
                ContentType = "image/jpeg",
                Length = bytes.Length,
                Content = new MemoryStream(bytes)
            };
        }

        public void Dispose()
        {
            if (Directory.Exists(_testFolder))
            {
                Directory.Delete(
                    _testFolder,
                    true);
            }
        }
        #endregion

    }
}
