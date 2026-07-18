using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Entities;
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
    public class MemberSubscriptionService : IMemberSubscriptionService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<MemberSubscriptionService> _logger;

        public MemberSubscriptionService(
            IAppDbContext appDbContext,
            ILogger<MemberSubscriptionService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(MemberSubscriptionDTO dto)
        {
            var validationResult = await ValidateMemberSubscriptionDTO(dto);

            if (!validationResult.IsSuccess)
            {
                return Result<int>.Failure(
                    validationResult.Code,
                    validationResult.StatusCode);
            }

            var entity = dto.ToEntity();

            try
            {
                _appDbContext.MemberSubscriptions.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberSubscriptionService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<MemberSubscriptionDTO>>> GetAllAsync()
        {
            try
            {
                var memberSubscriptions = await _appDbContext.MemberSubscriptions
                .Include(x => x.Member)
                .Include(x => x.SubscriptionType)
                .Select(m => m.ToDTO())
                .AsNoTracking()          
                .ToListAsync();

                return Result<IEnumerable<MemberSubscriptionDTO>>.Success(memberSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberSubscriptionService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<MemberSubscriptionDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<MemberSubscriptionDTO>> GetByIdAsync(int id)
        {
            try
            {
                var memberSubscription = await _appDbContext.MemberSubscriptions
                    .Include(x => x.Member)
                    .Include(x => x.SubscriptionType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (memberSubscription == null)
                {
                    return Result<MemberSubscriptionDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<MemberSubscriptionDTO>.Success(memberSubscription.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberSubscriptionService),
                   nameof(GetByIdAsync));

                return Result<MemberSubscriptionDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<MemberSubscriptionAddUpdateDTO>> GetMemberSubscriptionAddUpdateDTO(int? id = null)
        {
            var DTO = new MemberSubscriptionAddUpdateDTO();

            if (id.HasValue)
            {
                var memberSubscription = await _appDbContext.MemberSubscriptions
                    .Include(x => x.Member)
                    .Include(x => x.SubscriptionType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id.Value);

                if (memberSubscription is null)
                {
                    return Result<MemberSubscriptionAddUpdateDTO>.Failure(
                        ResultCodes.NotFound);
                }

                DTO.MemberSubscription = memberSubscription.ToDTO();
            }

            DTO.Members = await _appDbContext.Members
                .Select(x => new MemberSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                })
                .ToListAsync();

            DTO.SubscriptionTypes = await _appDbContext.SubscriptionTypes
                .Select(x => new SubscriptionTypeSearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    Price = x.Price,
                    DurationDays = x.DurationDays,
                })
                .ToListAsync();

            return Result<MemberSubscriptionAddUpdateDTO>.Success(DTO);

        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, MemberSubscriptionDTO dto)
        {
            dto.Id = id;

            var validationResult = await ValidateMemberSubscriptionDTO(dto);

            if (!validationResult.IsSuccess)
            {
                return Result<bool>.Failure(
                    validationResult.Code,
                    validationResult.StatusCode);
            }


            try
            {
                var memberSubscription = _appDbContext.MemberSubscriptions.FirstOrDefault(x => x.Id == id);

                if (memberSubscription == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }


                memberSubscription.MemberId = dto.MemberId;
                memberSubscription.SubscriptionTypeId = dto.SubscriptionTypeId;
                memberSubscription.StartDate = dto.StartDate;
                memberSubscription.EndDate = dto.EndDate;
                memberSubscription.Price = dto.Price;
                memberSubscription.Status = dto.Status;
                memberSubscription.UpdatedAt = DateTime.UtcNow;


                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberSubscriptionService),
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
                var memberSubscription = await _appDbContext.MemberSubscriptions.FirstOrDefaultAsync(x => x.Id == id);

                if (memberSubscription == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                memberSubscription.IsDeleted = true;
                memberSubscription.UpdatedAt = DateTime.UtcNow;
                memberSubscription.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberSubscriptionService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================

        private async Task<Result<bool>> ValidateMemberSubscriptionDTO(MemberSubscriptionDTO DTO)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            if (DTO.Price < 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidPrice,
                    400);
            }

            var hasOverlappingSubscription = await HasOverlappingSubscription(
                DTO.MemberId,
                DTO.StartDate,
                DTO.EndDate,
                DTO.Id);

            if (hasOverlappingSubscription)
            {
                return Result<bool>.Failure(
                    ResultCodes.SubscriptionOverlap,
                    400);
            }

            return Result<bool>.Success(true);

        }

        private async Task<bool> HasOverlappingSubscription(
            int memberId,
            DateOnly startDate,
            DateOnly endDate,
            int? excludeId = null)
        {
            return await _appDbContext.MemberSubscriptions
                .AnyAsync(x =>
                x.MemberId == memberId &&
                (excludeId == null || x.Id != excludeId) && 
                x.StartDate <= endDate &&
                x.EndDate >= startDate);
        }

        #endregion

    }
}
