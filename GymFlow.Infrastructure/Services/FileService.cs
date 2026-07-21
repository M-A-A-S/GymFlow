using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.File;
using GymFlow.Domain.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class FileService : IFileService
    {
        #region ========================= Fields & Properties =========================
        private readonly string _rootPath;
        private readonly FileValidationOptions _options;
        private readonly ILogger<FileService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public FileService(
            string rootPath,
            FileValidationOptions options,
            ILogger<FileService> logger)
        {
            _rootPath = rootPath;
            _options = options;
            _logger = logger;
        }

        #endregion

        #region ========================= Save =========================
        public async Task<Result<string?>> SaveAsync(
            FileUploadRequest? file,
            string? imageUrl,
            string folder,
            CancellationToken cancellationToken = default)
        {
            // User provided URL instead of uploading a file
            if (file == null)
            {
                return Result<string?>.Success(imageUrl);
            }

            var validation = Validate(file);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            try
            {
                var fileName = GenerateFileName(file.FileName);
                var directory = Path.Combine(_rootPath, folder);

                Directory.CreateDirectory(directory);

                var path = Path.Combine(directory, fileName);

                await using var stream = new FileStream(path, FileMode.Create);
                await file.Content.CopyToAsync(stream, cancellationToken);

                return Result<string?>.Success(
                    fileName,
                    ResultCodes.FileSaved);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(FileService),
                   nameof(SaveAsync));

                return Result<string?>.Failure(
                    ResultCodes.FileSaveFailed,
                    500, "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Replace =========================
        public async Task<Result<string?>> ReplaceAsync(
            FileUploadRequest? file, 
            string? imageUrl, 
            string? oldFileName, 
            string folder, 
            CancellationToken cancellationToken = default)
        {
            // Nothing changed
            if (file is null && string.IsNullOrWhiteSpace(imageUrl))
            {
                return Result<string?>.Success(oldFileName);
            }

            if (!string.IsNullOrWhiteSpace(oldFileName))
            {
                var deleteResult = await DeleteAsync(oldFileName, folder, cancellationToken);

                if (!deleteResult.IsSuccess)
                {
                    return Result<string?>.Failure(
                        deleteResult.Code,
                        deleteResult.StatusCode,
                        deleteResult.Message);
                }
            }

            return await SaveAsync(file, imageUrl, folder, cancellationToken);
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<Result<bool>> DeleteAsync(
            string? fileName, 
            string folder, 
            CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result<bool>.Success(true);
            }

            // External URL
            if (Uri.IsWellFormedUriString(fileName, UriKind.Absolute))
            {
                return Result<bool>.Success(true);
            }

            try
            {
                var safeFileName =
                    Path.GetFileName(fileName);

                var path = Path.Combine(_rootPath, folder, safeFileName);

                // Already deleted
                if (!File.Exists(path))
                {
                    return Result<bool>.Success(true);
                }

                File.Delete(path);

                return Result<bool>.Success(true, ResultCodes.FileDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(FileService),
                   nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.FileSaveFailed,
                    500, "An unexpected error occurred.");
            }

        }

        #endregion

        #region ========================= Helpers =========================
        private Result<string?> Validate(FileUploadRequest file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (_options.AllowedExtensions == null || 
                !_options.AllowedExtensions.Contains(extension))
            {
                return Result<string?>.Failure(
                    ResultCodes.FileInvalidExtension);
            }

            if (file.Length > _options.MaxFileSizeBytes)
            {
                return Result<string?>.Failure(
                    ResultCodes.FileTooLarge);
            }

            return Result<string?>.Success(null);
        }

        private static string GenerateFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid():N}{extension}";
        }

        #endregion

    }
}
