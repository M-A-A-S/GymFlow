using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using GymFlow.Infrastructure.Extensions;
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
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SubscriptionTypeService> _logger;
        private readonly IMemoryCache _cache;

        #endregion

        #region ========================= Constructors =========================
        public SubscriptionTypeService(
            IAppDbContext appDbContext,
            ILogger<SubscriptionTypeService> logger,
            IMemoryCache cache)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _cache = cache;
        }

        #endregion

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
                _cache.Remove(CacheKeys.SubscriptionTypesSelect);
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
                    HttpStatusCodes.InternalServerError,
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<SubscriptionTypeDTO>>> GetAllAsync(SubscriptionTypeFilterDTO filter)
        {
            try
            {

                var query =
                    _appDbContext.SubscriptionTypes
                    .AsNoTracking();

                // Search
                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                        x.NameEn.Contains(filter.Search) ||
                        x.NameEn.Contains(filter.Search)
                        );
                }

                // Active / inactive
                if (filter.IsActive.HasValue)
                {
                    query =
                        query.Where(x => x.IsActive == filter.IsActive.Value);
                }

                // Days per week
                if (filter.MinDaysPerWeek.HasValue)
                {
                    query =
                        query.Where(x => x.DaysPerWeek >= filter.MinDaysPerWeek.Value);
                }

                if (filter.MaxDaysPerWeek.HasValue)
                {
                    query =
                        query.Where(x => x.DaysPerWeek <= filter.MaxDaysPerWeek.Value);
                }

                // Price
                if (filter.MinPrice.HasValue)
                {
                    query =
                        query.Where(x => x.Price >= filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    query =
                        query.Where(x => x.Price <= filter.MaxPrice.Value);
                }

                // Duration
                if (filter.MinDurationDays.HasValue)
                {
                    query =
                        query.Where(x => x.DurationDays >= filter.MinDurationDays.Value);
                }

                if (filter.MaxDurationDays.HasValue)
                {
                    query =
                        query.Where(x => x.DurationDays <= filter.MaxDurationDays.Value);
                }

                if (filter.HasMembers.HasValue)
                {
                    if (filter.HasMembers.Value)
                    {
                        // Has at least one member
                        query = 
                            query.Where(x => x.MemberSubscriptions.Any());
                    }
                    else
                    {
                        query =
                            query.Where(x => !x.MemberSubscriptions.Any());
                    }
                }


                //query = query.OrderByDescending(x => x.Id);
                query = query.OrderByProperty(filter.SortBy, filter.Descending);

                var pagedResult = await query.ToPagedListAsync(filter.PageNumber, filter.PageSize);

                var result = new PagedResult<SubscriptionTypeDTO>
                {
                    Items = pagedResult.Items.Select(x => x.ToDTO()),
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount,
                    TotalPages = pagedResult.TotalPages,
                };


                return Result<PagedResult<SubscriptionTypeDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SubscriptionTypeService),
                   nameof(GetAllAsync));

                return Result<PagedResult<SubscriptionTypeDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
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
                    return Result<SubscriptionTypeDTO>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<SubscriptionTypeSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.SubscriptionTypes
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.NameEn.Contains(search) ||
                    x.NameAr.Contains(search));
            }

            var subscriptionTypes = await query
                .Take(20)
                .Select(x => new SubscriptionTypeSearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    Price = x.Price,
                    DurationDays = x.DurationDays,
                })
                .ToListAsync();

            return Result<IEnumerable<SubscriptionTypeSearchDTO>>.Success(subscriptionTypes);
        }

        public async Task<Result<IEnumerable<SubscriptionTypeSearchDTO>>> GetForSelectAsync()
        {
            try
            {
                if (_cache.TryGetValue(
                    CacheKeys.SubscriptionTypesSelect,
                    out IEnumerable<SubscriptionTypeSearchDTO>? subscriptionTypes))
                {
                    return Result<IEnumerable<SubscriptionTypeSearchDTO>>
                        .Success(subscriptionTypes);
                }


                subscriptionTypes = await _appDbContext.SubscriptionTypes
                    .AsNoTracking()
                    .OrderBy(x => x.NameEn)
                    .Select(x => new SubscriptionTypeSearchDTO
                    {
                        Id = x.Id,
                        NameEn = x.NameEn,
                        NameAr = x.NameAr,
                        Price = x.Price,
                        DurationDays = x.DurationDays,
                    })
                    .ToListAsync();


                _cache.Set(
                    CacheKeys.SubscriptionTypesSelect,
                    subscriptionTypes,
                    new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                    });


                return Result<IEnumerable<SubscriptionTypeSearchDTO>>
                    .Success(subscriptionTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading subscriptionTypes for select");

                return Result<IEnumerable<SubscriptionTypeSearchDTO>>
                    .Failure(
                        ResultCodes.UnexpectedError,
                        HttpStatusCodes.InternalServerError,
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
                    return Result<bool>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }


                subscriptionType.NameEn = dto.NameEn;
                subscriptionType.NameAr = dto.NameAr;
                subscriptionType.DaysPerWeek = dto.DaysPerWeek;
                subscriptionType.DurationDays = dto.DurationDays;
                subscriptionType.Price = dto.Price;
                subscriptionType.IsActive = dto.IsActive;
                subscriptionType.UpdatedAt = DateTime.UtcNow;


                await _appDbContext.SaveChangesAsync();
                _cache.Remove(CacheKeys.SubscriptionTypesSelect);
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
                    HttpStatusCodes.InternalServerError, "An unexpected error occurred.");
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
                        HttpStatusCodes.NotFound);
                }

                subscriptionType.IsDeleted = true;
                subscriptionType.UpdatedAt = DateTime.UtcNow;
                subscriptionType.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                _cache.Remove(CacheKeys.SubscriptionTypesSelect);
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
                    HttpStatusCodes.InternalServerError,
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
                    HttpStatusCodes.BadRequest);
            }

            if (DTO.DaysPerWeek < 1 || DTO.DaysPerWeek > 7)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidDaysPerWeek,
                    HttpStatusCodes.BadRequest);
            }

            if (DTO.DurationDays <= 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidDuration,
                    HttpStatusCodes.BadRequest);
            }

            if (DTO.Price < 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidPrice,
                    HttpStatusCodes.BadRequest);
            }

            return Result<bool>.Success(true);

        }

        #endregion
    
    }
}
