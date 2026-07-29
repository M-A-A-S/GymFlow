using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class SystemSettingService : ISystemSettingService
    {

        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SystemSettingService> _logger;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;
        private const string Cache_SystemSetting = "SystemSetting";

        #endregion

        #region ========================= Constructors =========================
        public SystemSettingService(
            IAppDbContext appDbContext,
            ILogger<SystemSettingService> logger,
            IFileService fileService,
            IMemoryCache cache)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _fileService = fileService;
            _cache = cache;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(SystemSettingDTO dto)
        {

            try
            {
                var validationResult = await ValidateSystemSettingDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var imageResult = await _fileService.SaveAsync(
                    dto.Image,
                    dto.LogoUrl,
                    Constants.SystemSettingsFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        imageResult.Code,
                        imageResult.StatusCode);
                }

                var entity = dto.ToEntity();

                entity.LogoUrl = imageResult.Data;

                _appDbContext.SystemSettings.Add(entity);
                await _appDbContext.SaveChangesAsync();
                _cache.Remove(Cache_SystemSetting);
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SystemSettingService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<SystemSettingDTO>>> GetAllAsync()
        {
            try
            {
                var systemSettings = await _appDbContext.SystemSettings
                    .AsNoTracking()
                    .Select(m => m.ToDTO())
                    .ToListAsync();

                return Result<IEnumerable<SystemSettingDTO>>.Success(systemSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SystemSettingService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<SystemSettingDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<SystemSettingDTO>> GetByIdAsync(int id)
        {
            try
            {
                var systemSetting = await _appDbContext.SystemSettings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (systemSetting == null)
                {
                    return Result<SystemSettingDTO>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }
                return Result<SystemSettingDTO>.Success(systemSetting.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SystemSettingService),
                   nameof(GetByIdAsync));

                return Result<SystemSettingDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<SystemSettingDTO>> GetCurrentAsync()
        {
            var setting = await _cache.GetOrCreateAsync(
                Cache_SystemSetting,
                async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(1);

                    return await _appDbContext.SystemSettings
                        .AsNoTracking()
                        .Select(x => x.ToDTO())
                        .FirstOrDefaultAsync();
                });

            if (setting == null)
            {
                return Result<SystemSettingDTO>.Failure(
                    ResultCodes.NotFound,
                    HttpStatusCodes.NotFound);
            }

            return Result<SystemSettingDTO>.Success(setting);
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, SystemSettingDTO dto)
        {

            try
            {
                var validationResult = await ValidateSystemSettingDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                //var entity = dto.ToEntity();

                var SystemSetting = _appDbContext.SystemSettings.FirstOrDefault(x => x.Id == id);

                if (SystemSetting == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }

                var imageResult = await _fileService.ReplaceAsync(
                    dto.Image,
                    dto.LogoUrl,
                    SystemSetting.LogoUrl,
                    Constants.SystemSettingsFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        imageResult.Code,
                        imageResult.StatusCode);
                }

                SystemSetting.LogoUrl = imageResult.Data;

                SystemSetting.NameEn = dto.NameEn;
                SystemSetting.NameAr = dto.NameAr;
                SystemSetting.AddressEn = dto.AddressEn;
                SystemSetting.AddressAr = dto.AddressAr;
                SystemSetting.Phone = dto.Phone;
                SystemSetting.Email = dto.Email;
                SystemSetting.Website = dto.Website;
                SystemSetting.Facebook = dto.Facebook;
                SystemSetting.Instagram = dto.Instagram;
                SystemSetting.TaxNumber = dto.TaxNumber;
                SystemSetting.Currency = dto.Currency;
                SystemSetting.ReceiptFooterEn = dto.ReceiptFooterEn;
                SystemSetting.ReceiptFooterAr = dto.ReceiptFooterAr;

                SystemSetting.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                _cache.Remove(Cache_SystemSetting);
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SystemSettingService),
                    nameof(UpdateAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError, "An unexpected error occurred.");
            }
        }
        #endregion

        #region ========================= Delete =========================
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var SystemSetting = await _appDbContext.SystemSettings.FirstOrDefaultAsync(x => x.Id == id);

                if (SystemSetting == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        HttpStatusCodes.NotFound);
                }

                var deleteFileResult = await _fileService.DeleteAsync(
                    SystemSetting.LogoUrl,
                    Constants.SystemSettingsFolder);

                if (!deleteFileResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        deleteFileResult.Code,
                        deleteFileResult.StatusCode);
                }

                SystemSetting.IsDeleted = true;
                SystemSetting.UpdatedAt = DateTime.UtcNow;
                SystemSetting.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                _cache.Remove(Cache_SystemSetting);
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SystemSettingService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateSystemSettingDTO(SystemSettingDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    HttpStatusCodes.BadRequest);
            }

            return Result<bool>.Success(true);

        }

        #endregion

    }
}
