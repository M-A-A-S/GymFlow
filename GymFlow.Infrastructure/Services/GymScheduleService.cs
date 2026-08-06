using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.GymSchedule;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using GymFlow.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class GymScheduleService : IGymScheduleService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<GymScheduleService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public GymScheduleService(
            IAppDbContext appDbContext,
            ILogger<GymScheduleService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(GymScheduleDTO dto)
        {

            try
            {
                var validationResult = await ValidateGymScheduleDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();
                entity.DurationHours = TimeHelper.CalculateDurationHours(
                    entity.StartTime,
                    entity.EndTime);

                _appDbContext.GymSchedules.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(GymScheduleService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<GymScheduleDTO>>> GetAllAsync()
        {
            try
            {
                var gymSchedules = await _appDbContext.GymSchedules
                .Select(x => x.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<GymScheduleDTO>>.Success(gymSchedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(GymScheduleService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<GymScheduleDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<GymScheduleDTO>>> GetAllAsync(GymScheduleFilterDTO filter)
        {
            try
            {
                var query = _appDbContext.GymSchedules
                .AsNoTracking();

                query = ApplyFilters(query, filter);

                query = ApplySorting(query, filter);

                var dtoQuery = ProjectToDTO(query);

                var pagedResult = await dtoQuery.ToPagedListAsync(
                    filter.PageNumber,
                    filter.PageSize);

                return Result<PagedResult<GymScheduleDTO>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(GymScheduleService),
                   nameof(GetAllAsync));

                return Result<PagedResult<GymScheduleDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<GymScheduleDTO>> GetByIdAsync(int id)
        {
            try
            {
                var gymSchedule = await _appDbContext.GymSchedules
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);


                if (gymSchedule == null)
                {
                    return Result<GymScheduleDTO>.Failure(ResultCodes.NotFound, 404);
                }

                return Result<GymScheduleDTO>.Success(gymSchedule.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(GymScheduleService),
                   nameof(GetByIdAsync));

                return Result<GymScheduleDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, GymScheduleDTO dto)
        {

            try
            {
                var validationResult = await ValidateGymScheduleDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                var gymSchedule = _appDbContext.GymSchedules.FirstOrDefault(x => x.Id == id);

                if (gymSchedule == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                gymSchedule.Day = dto.Day.Value;
                gymSchedule.StartTime = dto.StartTime.Value;
                gymSchedule.EndTime = dto.EndTime.Value;
                gymSchedule.Gender = dto.Gender.Value;
                gymSchedule.UpdatedAt = DateTime.UtcNow;

                gymSchedule.DurationHours = TimeHelper.CalculateDurationHours(
                    gymSchedule.StartTime,
                    gymSchedule.EndTime);

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(GymScheduleService),
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
                var gymSchedule = await _appDbContext.GymSchedules.FirstOrDefaultAsync(x => x.Id == id);

                if (gymSchedule == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                gymSchedule.IsDeleted = true;
                gymSchedule.UpdatedAt = DateTime.UtcNow;
                gymSchedule.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(GymScheduleService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateGymScheduleDTO(GymScheduleDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            if (DTO.StartTime >= DTO.EndTime)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidScheduleTime, 400);
            }

            var hasOverlap = await _appDbContext.GymSchedules
                .AnyAsync(x =>
                    x.Day == DTO.Day &&
                    (excludedId == null || x.Id != excludedId) &&
                    DTO.StartTime < x.EndTime && // new starts before existing ends
                    DTO.EndTime > x.StartTime); // new ends after existing starts

            if (hasOverlap)
            {
                return Result<bool>.Failure(
                    ResultCodes.GymScheduleOverlap, 400);
            }

            return Result<bool>.Success(true);

        }

        private IQueryable<GymSchedule> ApplyFilters(
            IQueryable<GymSchedule> query,
            GymScheduleFilterDTO filter)
        {

            // ========================== Gender ==========================
            if (filter.Gender.HasValue)
            {
                query = query.Where(x =>
                    x.Gender == filter.Gender.Value);
            }

            // ========================== Day ==========================
            if (filter.Day.HasValue)
            {
                query = query.Where(x =>
                    x.Day == filter.Day.Value);
            }

            // ========================== Start Time ==========================
            if (filter.StartTimeFrom.HasValue)
            {
                query = query.Where(x =>
                    x.StartTime >= filter.StartTimeFrom.Value);
            }

            if (filter.StartTimeTo.HasValue)
            {
                query = query.Where(x =>
                    x.StartTime <= filter.StartTimeTo.Value);
            }

            // ========================== End Time ==========================
            if (filter.EndTimeFrom.HasValue)
            {
                query = query.Where(x =>
                    x.EndTime >= filter.EndTimeFrom.Value);
            }

            if (filter.EndTimeTo.HasValue)
            {
                query = query.Where(x =>
                    x.EndTime <= filter.EndTimeTo.Value);
            }

            // ========================== Duration Hours ==========================
            if (filter.MinDurationHours.HasValue)
            {
                query = query.Where(x =>
                    x.DurationHours >= filter.MinDurationHours.Value);
            }

            if (filter.MaxDurationHours.HasValue)
            {
                query = query.Where(x =>
                    x.DurationHours <= filter.MaxDurationHours.Value);
            }

            return query;

        }

        private IQueryable<GymSchedule> ApplySorting(
            IQueryable<GymSchedule> query,
            GymScheduleFilterDTO filter)
        {
            bool desc = filter.Descending;

            switch (filter.SortBy)
            {
                // ========================= Member ========================= 
                // From Saturday to Firday
                case "Day":
                    return desc
                        ? query.OrderByDescending(x =>
                            x.Day == DayOfWeek.Saturday ? 0 :
                            x.Day == DayOfWeek.Sunday ? 1 :
                            x.Day == DayOfWeek.Monday ? 2 :
                            x.Day == DayOfWeek.Tuesday ? 3 :
                            x.Day == DayOfWeek.Wednesday ? 4 :
                            x.Day == DayOfWeek.Thursday ? 5 :
                            6)
                        : query.OrderBy(x =>
                            x.Day == DayOfWeek.Saturday ? 0 :
                            x.Day == DayOfWeek.Sunday ? 1 :
                            x.Day == DayOfWeek.Monday ? 2 :
                            x.Day == DayOfWeek.Tuesday ? 3 :
                            x.Day == DayOfWeek.Wednesday ? 4 :
                            x.Day == DayOfWeek.Thursday ? 5 :
                            6);

                // ========================= Entity Properties =========================
                default:
                    return query.OrderByProperty(filter.SortBy, desc);
            }

        }

        private IQueryable<GymScheduleDTO> ProjectToDTO(
            IQueryable<GymSchedule> query
            )
        {
            return query.Select(x => new GymScheduleDTO
            {
                Id = x.Id,
                Gender = x.Gender,
                Day = x.Day,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DurationHours = x.DurationHours,
            });
        }

        #endregion

    }
}
