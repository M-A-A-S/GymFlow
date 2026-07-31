using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using GymFlow.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class MemberSubscriptionService : IMemberSubscriptionService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<MemberSubscriptionService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public MemberSubscriptionService(
            IAppDbContext appDbContext,
            ILogger<MemberSubscriptionService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

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
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                var memberSubscriptions = await _appDbContext.MemberSubscriptions
                .AsNoTracking()
                .OrderByDescending(x => x.StartDate)
                .Select(m => new MemberSubscriptionDTO
                {
                    Id = m.Id,
                    MemberId = m.MemberId,
                    SubscriptionTypeId = m.SubscriptionTypeId,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    Status = m.Status,
                    Price = m.Price,

                    // Include both start and end dates in duration calculation
                    ActualDurationDays = m.EndDate.DayNumber - m.StartDate.DayNumber + 1,

                    RemainingDays = m.EndDate >= today
                        ? m.EndDate.DayNumber - today.DayNumber + 1
                        : 0,

                    AttendanceDays = m.Member.MemberAttendances
                        .Where(a =>
                            a.AttendanceDate >= m.StartDate &&
                            a.AttendanceDate <= m.EndDate)
                        .Select(a => a.AttendanceDate)
                        .Distinct()
                        .Count(),

                    LastAttendanceDate = m.Member.MemberAttendances
                        .Where(x => 
                            x.AttendanceDate >= m.StartDate &&
                            x.AttendanceDate <= m.EndDate)
                        .Max(x => (DateOnly?)x.AttendanceDate),


                    //IsStarted = m.StartDate <= today,

                    //IsCurrent = 
                    //    m.Status == SubscriptionStatus.Active &&
                    //    m.StartDate <= today &&
                    //    m.EndDate >= today,

                    //IsExpired = m.EndDate < today,

                    TimeStatus =
                            m.EndDate < today
                                ? SubscriptionTimeStatus.Expired
                                : m.StartDate > today
                                    ? SubscriptionTimeStatus.Upcoming
                                    : SubscriptionTimeStatus.Current,


                    Member = new MemberDTO
                    {
                        Id = m.Member.Id,
                        FullName = m.Member.FullName,
                        PhoneNumber = m.Member.PhoneNumber,
                        Gender = m.Member.Gender,
                    },

                    SubscriptionType = new SubscriptionTypeDTO
                    {
                        Id = m.SubscriptionType.Id,
                        NameEn = m.SubscriptionType.NameEn,
                        NameAr = m.SubscriptionType.NameAr,
                        Price = m.SubscriptionType.Price,
                        DurationDays = m.SubscriptionType.DurationDays,

                    },
                })        
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

        public async Task<Result<PagedResult<MemberSubscriptionDTO>>> GetAllAsync(MemberSubscriptionFilterDTO filter)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                var query =
                    _appDbContext.MemberSubscriptions
                    .AsNoTracking();

                //query = ApplyFilters(query, filter, today);
                //query = ApplySorting(query, filter, today);

                //query = query.OrderByProperty(filter.SortBy, filter.Descending);



                //query = query.OrderByDescending(x => x.Id);


                //var pagedResult = await query.ToPagedListAsync(filter.PageNumber, filter.PageSize);
                //var pagedResult = await ProjectToDTO(query, today).ToPagedListAsync(filter.PageNumber, filter.PageSize);

                //var result = new PagedResult<MemberSubscriptionDTO>
                //{
                //    Items = pagedResult.Items,
                //    PageNumber = pagedResult.PageNumber,
                //    PageSize = pagedResult.PageSize,
                //    TotalCount = pagedResult.TotalCount,
                //    TotalPages = pagedResult.TotalPages,
                //};

                query = ApplyFilters(query, filter, today);

                var dtoQuery = ProjectToDTO(query, today);

                dtoQuery = ApplyDtoSorting(dtoQuery, filter);

                var pagedResult = await dtoQuery.ToPagedListAsync(
                    filter.PageNumber,
                    filter.PageSize);


                return Result<PagedResult<MemberSubscriptionDTO>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberSubscriptionService),
                   nameof(GetAllAsync));

                return Result<PagedResult<MemberSubscriptionDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<MemberSubscriptionDTO>> GetByIdAsync(int id)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                var memberSubscription = await _appDbContext.MemberSubscriptions
                    .AsNoTracking()
                    .Select(m => new MemberSubscriptionDTO
                    {
                        Id = m.Id,
                        MemberId = m.MemberId,
                        SubscriptionTypeId = m.SubscriptionTypeId,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate,
                        Status = m.Status,
                        Price = m.Price,

                        // Include both start and end dates in duration calculation
                        ActualDurationDays = m.EndDate.DayNumber - m.StartDate.DayNumber + 1,

                        RemainingDays = m.EndDate >= today
                        ? m.EndDate.DayNumber - today.DayNumber + 1
                        : 0,

                        AttendanceDays = m.Member.MemberAttendances
                        .Where(a =>
                            a.AttendanceDate >= m.StartDate &&
                            a.AttendanceDate <= m.EndDate)
                        .Select(a => a.AttendanceDate)
                        .Distinct()
                        .Count(),

                        LastAttendanceDate = m.Member.MemberAttendances
                        .Where(x =>
                            x.AttendanceDate >= m.StartDate &&
                            x.AttendanceDate <= m.EndDate)
                        .Max(x => (DateOnly?)x.AttendanceDate),


                        //IsStarted = m.StartDate <= today,

                        //IsCurrent =
                        //m.Status == SubscriptionStatus.Active &&
                        //m.StartDate <= today &&
                        //m.EndDate >= today,

                        //IsExpired = m.EndDate < today,

                        TimeStatus =
                            m.EndDate < today
                                ? SubscriptionTimeStatus.Expired
                                : m.StartDate > today
                                    ? SubscriptionTimeStatus.Upcoming
                                    : SubscriptionTimeStatus.Current,


                        Member = m.Member == null ? null : new MemberDTO
                        {
                            Id = m.Member.Id,
                            FullName = m.Member.FullName,
                            PhoneNumber = m.Member.PhoneNumber,
                            Gender = m.Member.Gender,
                        },

                        SubscriptionType = m.SubscriptionType == null ? null : new SubscriptionTypeDTO
                        {
                            Id = m.SubscriptionType.Id,
                            NameEn = m.SubscriptionType.NameEn,
                            NameAr = m.SubscriptionType.NameAr,
                            Price = m.SubscriptionType.Price,
                            DurationDays = m.SubscriptionType.DurationDays,

                        },
                    }) 
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (memberSubscription == null)
                {
                    return Result<MemberSubscriptionDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<MemberSubscriptionDTO>.Success(memberSubscription);
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

        private IQueryable<MemberSubscription> ApplyFilters(
            IQueryable<MemberSubscription> query,
            MemberSubscriptionFilterDTO filter,
            DateOnly today)
        {
            // ========================== Search ==========================
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x =>
                    x.Member.FullName.Contains(filter.Search) ||
                    x.Member.PhoneNumber.Contains(filter.Search) ||
                    x.SubscriptionType.NameEn.Contains(filter.Search) ||
                    x.SubscriptionType.NameAr.Contains(filter.Search)
                    );
            }

            // ========================== Status ==========================
            if (filter.Status.HasValue)
            {
                query =
                    query.Where(x => x.Status == filter.Status.Value);
            }

            // ========================== Time Status ==========================
            if (filter.TimeStatus.HasValue)
            {
                switch (filter.TimeStatus.Value)
                {
                    case SubscriptionTimeStatus.Expired:
                        query = query.Where(x =>
                            x.EndDate < today);
                        break;

                    case SubscriptionTimeStatus.Upcoming:
                        query = query.Where(x =>
                            x.StartDate > today);
                        break;

                    case SubscriptionTimeStatus.Current:
                        query = query.Where(x =>
                            x.StartDate <= today &&
                            x.EndDate >= today);
                        break;
                }
            }

            // ========================== Subscription Type ==========================
            if (filter.SubscriptionTypeId.HasValue)
            {
                query =
                    query.Where(x => x.SubscriptionTypeId == filter.SubscriptionTypeId.Value);
            }

            // ========================== Member ==========================
            if (filter.MemberId.HasValue)
            {
                query =
                    query.Where(x => x.MemberId == filter.MemberId.Value);
            }

            // ========================== Start Date ==========================
            if (filter.StartDateFrom.HasValue)
            {
                query =
                    query.Where(x => x.StartDate >= filter.StartDateFrom.Value);
            }

            if (filter.StartDateTo.HasValue)
            {
                query =
                    query.Where(x => x.StartDate <= filter.StartDateTo.Value);
            }

            // ========================== End Date ==========================
            if (filter.EndDateFrom.HasValue)
            {
                query =
                    query.Where(x => x.EndDate >= filter.EndDateFrom.Value);
            }

            if (filter.EndDateTo.HasValue)
            {
                query =
                    query.Where(x => x.EndDate <= filter.EndDateTo.Value);
            }


            // ========================== Price ==========================
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

            // ========================== Expiring Soon ==========================
            // subscriptions ending within 7 days
            if (filter.ExpiringSoon == true)
            {
                var limitDate = today.AddDays(7);

                query =
                    query.Where(x =>
                        x.EndDate >= today &&
                        x.EndDate <= limitDate);
            }

            // ========================== Actual Duration Days ==========================
            if (filter.MinDurationDays.HasValue)
            {
                query = query.Where(x =>
                    x.EndDate >= x.StartDate.AddDays(filter.MinDurationDays.Value - 1));
            }

            if (filter.MaxDurationDays.HasValue)
            {
                query = query.Where(x =>
                    x.EndDate <= x.StartDate.AddDays(filter.MaxDurationDays.Value - 1));
            }
            //if (filter.MinDurationDays.HasValue)
            //{
            //    query =
            //        query.Where(x =>
            //            (x.EndDate.DayNumber - x.StartDate.DayNumber + 1) >= filter.MinDurationDays.Value);
            //}

            //if (filter.MaxDurationDays.HasValue)
            //{
            //    query =
            //        query.Where(x =>
            //            (x.EndDate.DayNumber - x.StartDate.DayNumber + 1) <= filter.MaxDurationDays.Value);
            //}

            // ========================== Remaining Days ==========================
            if (filter.MinRemainingDays.HasValue)
            {
                query = query.Where(x =>
                    x.EndDate >= today.AddDays(filter.MinRemainingDays.Value - 1));
            }

            if (filter.MaxRemainingDays.HasValue)
            {
                query = query.Where(x =>
                    x.EndDate <= today.AddDays(filter.MaxRemainingDays.Value - 1));
            }
            //if (filter.MinRemainingDays.HasValue)
            //{
            //    query =
            //        query.Where(x =>
            //            (x.EndDate.DayNumber - today.DayNumber + 1) >= filter.MinRemainingDays.Value);
            //}

            //if (filter.MaxRemainingDays.HasValue)
            //{
            //    query =
            //        query.Where(x =>
            //            (x.EndDate.DayNumber - today.DayNumber + 1) <= filter.MaxRemainingDays.Value);
            //}

            // ========================== Attendance Days ==========================
            if (filter.MinAttendanceDays.HasValue)
            {
                query =
                    query.Where(x => 
                        x.Member.MemberAttendances
                            .Count(a => 
                                a.AttendanceDate >= x.StartDate &&
                                a.AttendanceDate <= x.EndDate)
                        >= filter.MinAttendanceDays.Value);
            }

            if (filter.MaxAttendanceDays.HasValue)
            {
                query =
                    query.Where(x => 
                        x.Member.MemberAttendances
                            .Count(a => 
                                a.AttendanceDate >= x.StartDate &&
                                a.AttendanceDate <= x.EndDate)
                        <= filter.MaxAttendanceDays.Value);
            }


            // ========================== Attendance Days ==========================
            if (filter.LastAttendanceFrom.HasValue)
            {
                query = query.Where(x =>
                    x.Member.MemberAttendances
                    .Any(a =>
                        a.AttendanceDate >=
                        filter.LastAttendanceFrom.Value));
            }

            if (filter.LastAttendanceTo.HasValue)
            {
                query = query.Where(x =>
                    x.Member.MemberAttendances
                    .Any(a =>
                        a.AttendanceDate <=
                        filter.LastAttendanceTo.Value));
            }

            // ========================== Has Attendance ==========================
            if (filter.HasAttendance.HasValue)
            {

                if (filter.HasAttendance.Value)
                {
                    query = query.Where(x =>
                        x.Member.MemberAttendances.Any(a =>
                            a.AttendanceDate >= x.StartDate &&
                            a.AttendanceDate <= x.EndDate));
                }
                else
                {
                    query = query.Where(x =>
                        !x.Member.MemberAttendances.Any(a =>
                            a.AttendanceDate >= x.StartDate &&
                            a.AttendanceDate <= x.EndDate));
                }

            }

            return query;


        }

        private IQueryable<MemberSubscriptionDTO> ProjectToDTO(
            IQueryable<MemberSubscription> query,
            DateOnly today)
        {
            return query.Select(x => new MemberSubscriptionDTO
            {
                Id = x.Id,
                MemberId = x.MemberId,
                SubscriptionTypeId = x.SubscriptionTypeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Price = x.Price,
                Status = x.Status,
                //ActualDurationDays = x.EndDate.DayNumber - x.StartDate.DayNumber + 1,

                //RemainingDays = x.EndDate >= today ? x.EndDate.DayNumber - today.DayNumber + 1 : 0,

                // Include both start and end dates in duration calculation
                // The +1 is because you count both the start day and the end day.
                //Example:
                //StartDate = 2026 - 07 - 01
                //EndDate = 2026 - 07 - 01
                //Without + 1:
                //EndDate - StartDate = 0
                //But the subscription lasted 1 day,
                //so:
                //0 + 1 = 1 day
                            ActualDurationDays =
                EF.Functions.DateDiffDay(
                    x.StartDate,
                    x.EndDate) + 1,


                            RemainingDays =
                x.EndDate >= today
                    ? EF.Functions.DateDiffDay(
                        today,
                        x.EndDate) + 1
                    : 0,

                AttendanceDays = 
                    x.Member.MemberAttendances
                    .Count(a => 
                        a.AttendanceDate >= x.StartDate && 
                        a.AttendanceDate <= x.EndDate),

                LastAttendanceDate = 
                    x.Member.MemberAttendances
                    .Where(a => 
                        a.AttendanceDate >= x.StartDate && 
                        a.AttendanceDate <= x.EndDate)
                    .Max(a =>
                        (DateOnly?)a.AttendanceDate),

                TimeStatus =
                    x.EndDate < today
                        ? SubscriptionTimeStatus.Expired
                        : x.StartDate > today
                            ? SubscriptionTimeStatus.Upcoming
                            : SubscriptionTimeStatus.Current,


                Member = new MemberDTO
                {
                    Id = x.Member.Id,
                    FullName = x.Member.FullName,
                    PhoneNumber = x.Member.PhoneNumber,
                    Gender = x.Member.Gender,
                },

                SubscriptionType = new SubscriptionTypeDTO
                {
                    Id = x.SubscriptionType.Id,
                    NameEn = x.SubscriptionType.NameEn,
                    NameAr = x.SubscriptionType.NameAr,
                    Price = x.SubscriptionType.Price,
                    DurationDays = x.SubscriptionType.DurationDays,

                },
            });
        }

        private IQueryable<MemberSubscription> ApplySorting(
            IQueryable<MemberSubscription> query,
            MemberSubscriptionFilterDTO filter,
            DateOnly today)
        {
            if (string.IsNullOrWhiteSpace(filter.SortBy))
            {
                return query.OrderByDescending(x => x.Id);
            }

            bool desc = filter.Descending;
            bool isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            switch (filter.SortBy)
            {
                // ========================= Member ========================= 
                case "Member":
                    return desc
                        ? query.OrderByDescending(x => x.Member.FullName)
                        : query.OrderBy(x => x.Member.FullName);

                // ========================= Member ========================= 
                case "SubscriptionType":
                    if (isArabic)
                    {
                        return desc
                            ? query.OrderByDescending(x => x.SubscriptionType.NameAr)
                            : query.OrderBy(x => x.SubscriptionType.NameAr);
                    }
                    else
                    {
                        return desc
                            ? query.OrderByDescending(x => x.SubscriptionType.NameEn)
                            : query.OrderBy(x => x.SubscriptionType.NameEn);
                    }

                // ========================= Member ========================= 
                case "ActualDurationDays":
                    return desc
                        ? query.OrderByDescending(x =>
                            x.EndDate.DayNumber - x.StartDate.DayNumber + 1)
                        : query.OrderBy(x =>
                            x.EndDate.DayNumber - x.StartDate.DayNumber + 1);

                // ========================= Member ========================= 
                case "RemainingDays":
                    return desc
                        ? query.OrderByDescending(x =>
                            x.EndDate < today
                                ? 0
                                : x.EndDate.DayNumber - today.DayNumber + 1)
                        : query.OrderBy(x =>
                            x.EndDate < today
                                ? 0
                                : x.EndDate.DayNumber - today.DayNumber + 1);

                // ========================= Member ========================= 
                case "AttendanceDays":
                    return desc
                        ? query.OrderByDescending(x =>
                            x.Member.MemberAttendances.Count(a =>
                                a.AttendanceDate >= x.StartDate &&
                                a.AttendanceDate <= x.EndDate))
                        : query.OrderBy(x =>
                            x.Member.MemberAttendances.Count(a =>
                                a.AttendanceDate >= x.StartDate &&
                                a.AttendanceDate <= x.EndDate));

                // ========================= Member ========================= 
                case "LastAttendanceDate":
                    return desc
                        ? query.OrderByDescending(x =>
                            x.Member.MemberAttendances
                                .Where(a =>
                                    a.AttendanceDate >= x.StartDate &&
                                    a.AttendanceDate <= x.EndDate)
                                .Max(a => (DateOnly?)a.AttendanceDate))
                        : query.OrderBy(x =>
                            x.Member.MemberAttendances
                                .Where(a =>
                                    a.AttendanceDate >= x.StartDate &&
                                    a.AttendanceDate <= x.EndDate)
                                .Max(a => (DateOnly?)a.AttendanceDate));

                // ========================= Time Status ========================= 
                case "TimeStatus":
                    return filter.Descending
                        ? query.OrderByDescending(x =>
                            (int)(
                                x.EndDate < today
                                    ? SubscriptionTimeStatus.Expired
                                    : x.StartDate > today
                                        ? SubscriptionTimeStatus.Upcoming
                                        : SubscriptionTimeStatus.Current))
                        : query.OrderBy(x =>
                            (int)(
                                x.EndDate < today
                                    ? SubscriptionTimeStatus.Expired
                                    : x.StartDate > today
                                        ? SubscriptionTimeStatus.Upcoming
                                        : SubscriptionTimeStatus.Current));

                // ========================= Entity Properties ========================= 
                default:
                    return query.OrderByProperty(filter.SortBy, desc);
            }

        }

        private IQueryable<MemberSubscriptionDTO> ApplyDtoSorting(
            IQueryable<MemberSubscriptionDTO> query, 
            MemberSubscriptionFilterDTO filter)
        {
            return filter.SortBy switch
            {
                "ActualDurationDays" => filter.Descending
                    ? query.OrderByDescending(x => x.ActualDurationDays)
                    : query.OrderBy(x => x.ActualDurationDays),

                "RemainingDays" => filter.Descending
                    ? query.OrderByDescending(x => x.RemainingDays)
                    : query.OrderBy(x => x.RemainingDays),

                "AttendanceDays" => filter.Descending
                    ? query.OrderByDescending(x => x.AttendanceDays)
                    : query.OrderBy(x => x.AttendanceDays),

                "LastAttendanceDate" => filter.Descending
                    ? query.OrderByDescending(x => x.LastAttendanceDate)
                    : query.OrderBy(x => x.LastAttendanceDate),

                "TimeStatus" => filter.Descending
                    ? query.OrderByDescending(x => x.TimeStatus)
                    : query.OrderBy(x => x.TimeStatus),

                "Member" => filter.Descending
                    ? query.OrderByDescending(x => x.Member!.FullName)
                    : query.OrderBy(x => x.Member!.FullName),

                "SubscriptionType" => filter.Descending
                    ? query.OrderByDescending(x => x.SubscriptionType!.NameEn)
                    : query.OrderBy(x => x.SubscriptionType!.NameEn),

                _ => query.OrderByProperty(filter.SortBy, filter.Descending)
            };
        }

        #endregion

    }
}
