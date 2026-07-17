using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SubscriptionTypeService> _logger;

        public SubscriptionTypeService(
            IAppDbContext appDbContext,
            ILogger<SubscriptionTypeService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(SubscriptionTypeDTO dto)
        {
            var validationResult = ValidateSubscriptionTypeDTO(dto);

            if (!validationResult.IsSuccess)
            {
                return Result<int>.Failure(
                    validationResult.Code,
                    validationResult.StatusCode);
            }

            var entity = dto.ToEntity();

            try
            {
                _appDbContext.SubscriptionTypes.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SubscriptionTypeService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<SubscriptionTypeDTO>>> GetAllAsync()
        {
            try
            {
                var subscriptionTypes = await _appDbContext.SubscriptionTypes
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<SubscriptionTypeDTO>>.Success(subscriptionTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SubscriptionTypeService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<SubscriptionTypeDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<SubscriptionTypeDTO>> GetByIdAsync(int id)
        {
            try
            {
                var subscriptionType = await _appDbContext.SubscriptionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (subscriptionType == null)
                {
                    return Result<SubscriptionTypeDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<SubscriptionTypeDTO>.Success(subscriptionType.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SubscriptionTypeService),
                   nameof(GetByIdAsync));

                return Result<SubscriptionTypeDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, SubscriptionTypeDTO dto)
        {

            var validationResult = ValidateSubscriptionTypeDTO(dto);

            if (!validationResult.IsSuccess)
            {
                return Result<bool>.Failure(
                    validationResult.Code,
                    validationResult.StatusCode);
            }


            try
            {
                var subscriptionType = _appDbContext.SubscriptionTypes.FirstOrDefault(x => x.Id == id);

                if (subscriptionType == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }


                subscriptionType.NameEn = dto.NameEn;
                subscriptionType.NameAr = dto.NameAr;
                subscriptionType.DaysPerWeek = dto.DaysPerWeek;
                subscriptionType.DurationDays = dto.DurationDays;
                subscriptionType.Price = dto.Price;
                subscriptionType.IsActive = dto.IsActive;
                subscriptionType.UpdatedAt = DateTime.UtcNow;


                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SubscriptionTypeService),
                    nameof(UpdateAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500, "An unexpected error occurred.");
            }
        }
        #endregion

        #region ========================= Delete =========================
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var subscriptionType = await _appDbContext.SubscriptionTypes.FirstOrDefaultAsync(x => x.Id == id);

                if (subscriptionType == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                subscriptionType.IsDeleted = true;
                subscriptionType.UpdatedAt = DateTime.UtcNow;
                subscriptionType.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SubscriptionTypeService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================

        private Result<bool> ValidateSubscriptionTypeDTO(SubscriptionTypeDTO DTO)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            if (DTO.DaysPerWeek < 1 || DTO.DaysPerWeek > 7)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidDaysPerWeek,
                    400);
            }

            if (DTO.DurationDays <= 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidDuration,
                    400);
            }

            if (DTO.Price < 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidPrice,
                    400);
            }

            return Result<bool>.Success(true);

        }

        #endregion
    
    }
}
