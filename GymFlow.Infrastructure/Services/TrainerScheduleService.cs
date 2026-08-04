using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Entities;
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
    public class TrainerScheduleService : ITrainerScheduleService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<TrainerScheduleService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public TrainerScheduleService(
            IAppDbContext appDbContext,
            ILogger<TrainerScheduleService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(TrainerScheduleDTO dto)
        {

            try
            {
                var validationResult = await ValidateTrainerScheduleDTO(dto);

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

                _appDbContext.TrainerSchedules.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerScheduleService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<TrainerScheduleDTO>>> GetAllAsync()
        {
            try
            {
                var trainerSchedules = await _appDbContext.TrainerSchedules
                .Select(x => new TrainerScheduleDTO
                {
                    Id = x.Id,
                    TrainerId = x.TrainerId,
                    Day = x.Day,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    DurationHours = x.DurationHours,

                    Trainer = new TrainerDTO
                    {
                        Id = x.Trainer.Id,
                        FullName = x.Trainer.FullName,
                        PhoneNumber = x.Trainer.PhoneNumber,  
                        Salary = x.Trainer.Salary,
                        HireDate = x.Trainer.HireDate,
                    }
                })
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<TrainerScheduleDTO>>.Success(trainerSchedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerScheduleService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<TrainerScheduleDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<TrainerScheduleDTO>>> GetAllAsync(TrainerScheduleFilterDTO filter)
        {
            try
            {
                var query = _appDbContext.TrainerSchedules
                .AsNoTracking();

                query = ApplyFilters(query, filter);

                query = ApplySorting(query, filter);

                var dtoQuery = ProjectToDTO(query);

                var pagedResult = await dtoQuery.ToPagedListAsync(
                    filter.PageNumber,
                    filter.PageSize);

                return Result<PagedResult<TrainerScheduleDTO>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerScheduleService),
                   nameof(GetAllAsync));

                return Result<PagedResult<TrainerScheduleDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<TrainerScheduleDTO>> GetByIdAsync(int id)
        {
            try
            {
                var trainerSchedule = await _appDbContext.TrainerSchedules
                    .Include(x => x.Trainer)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                

                if (trainerSchedule == null)
                {
                    return Result<TrainerScheduleDTO>.Failure(ResultCodes.NotFound, 404);
                }

                var result = trainerSchedule.ToDTO();
                result.Trainer = trainerSchedule.Trainer.ToDTO();

                return Result<TrainerScheduleDTO>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerScheduleService),
                   nameof(GetByIdAsync));

                return Result<TrainerScheduleDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<TrainerScheduleAddUpdateDTO>> GetTrainerScheduleAddUpdateDTO(int? id = null)
        {
            var DTO = new TrainerScheduleAddUpdateDTO();

            if (id.HasValue)
            {
                var trainerSchedule = await _appDbContext.TrainerSchedules
                    .Include(x => x.Trainer)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id.Value);

                if (trainerSchedule is null)
                {
                    return Result<TrainerScheduleAddUpdateDTO>.Failure(
                        ResultCodes.NotFound);
                }

                DTO.TrainerSchedule = trainerSchedule.ToDTO();
            }

            DTO.Trainers = await _appDbContext.Trainers
                .Select(x => new TrainerSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync();

            return Result<TrainerScheduleAddUpdateDTO>.Success(DTO);

        }   

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, TrainerScheduleDTO dto)
        {

            try
            {
                var validationResult = await ValidateTrainerScheduleDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                var trainerSchedule = _appDbContext.TrainerSchedules.FirstOrDefault(x => x.Id == id);

                if (trainerSchedule == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                

                trainerSchedule.TrainerId = dto.TrainerId;
                trainerSchedule.Day = dto.Day.Value;
                trainerSchedule.StartTime = dto.StartTime.Value;
                trainerSchedule.EndTime = dto.EndTime.Value;
                trainerSchedule.UpdatedAt = DateTime.UtcNow;

                trainerSchedule.DurationHours = TimeHelper.CalculateDurationHours(
                    trainerSchedule.StartTime,
                    trainerSchedule.EndTime);

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(TrainerScheduleService),
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
                var trainerSchedule = await _appDbContext.TrainerSchedules.FirstOrDefaultAsync(x => x.Id == id);

                if (trainerSchedule == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                trainerSchedule.IsDeleted = true;
                trainerSchedule.UpdatedAt = DateTime.UtcNow;
                trainerSchedule.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(TrainerScheduleService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateTrainerScheduleDTO(TrainerScheduleDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            if (DTO.TrainerId < 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.ValueCannotBeNegative,
                    400);
            }

            bool trainerExists =
                await _appDbContext.Trainers
                .AnyAsync(x => x.Id == DTO.TrainerId);

            if (!trainerExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.TrainerNotFound, 404);
            }

            if (DTO.StartTime >= DTO.EndTime)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidScheduleTime, 400);
            }

            var hasOverlap = await _appDbContext.TrainerSchedules
                .AnyAsync(x =>
                    x.TrainerId == DTO.TrainerId &&
                    x.Day == DTO.Day &&
                    (excludedId == null || x.Id != excludedId) &&
                    DTO.StartTime < x.EndTime && // new starts before existing ends
                    DTO.EndTime > x.StartTime); // new ends after existing starts

            if (hasOverlap)
            {
                return Result<bool>.Failure(
                    ResultCodes.TrainerScheduleOverlap, 400);
            }

            return Result<bool>.Success(true);

        }

        private IQueryable<TrainerSchedule> ApplyFilters(
            IQueryable<TrainerSchedule> query,
            TrainerScheduleFilterDTO filter)
        {
            // ========================== Search ==========================
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.Trainer.FullName.Contains(filter.Search));
            }

            // ========================== Trainer ==========================
            if (filter.TrainerId.HasValue)
            {
                query = query.Where(x =>
                    x.TrainerId == filter.TrainerId.Value);
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


            //if (filter.MinDurationHours.HasValue)
            //{
            //    query = query.Where(x => (x.EndTime - x.StartTime) < TimeSpan.Zero ? ((x.EndTime - x.StartTime) = (x.EndTime - x.StartTime) + TimeSpan.FromDays(1)) : )
            //}
            //if (filter.MinDurationHours.HasValue)
            //{
            //    query = query.Where(x =>
            //    {
            //        var duration = x.EndTime - x.StartTime;

            //        if (duration < TimeSpan.Zero)
            //        {
            //            duration += TimeSpan.FromDays(1);
            //        }

            //        return duration.
            //    }

            //}

            //if (filter.MaxDurationHours.HasValue)
            //{
            //    query = query.Where(x =>
            //        (x.EndTime - x.StartTime).TotalHours <= filter.MaxDurationHours.Value);
            //}

            return query;

        }

        private IQueryable<TrainerSchedule> ApplySorting(
            IQueryable<TrainerSchedule> query,
            TrainerScheduleFilterDTO filter)
        {
            bool desc = filter.Descending;

            switch (filter.SortBy)
            {
                // ========================= Member ========================= 
                case "Trainer":
                    return desc
                        ? query.OrderByDescending(x => x.Trainer.FullName)
                        : query.OrderBy(x => x.Trainer.FullName);
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

        private IQueryable<TrainerScheduleDTO> ProjectToDTO(
            IQueryable<TrainerSchedule> query
            )
        {
            return query.Select(x => new TrainerScheduleDTO
            {
                Id = x.Id,
                TrainerId = x.TrainerId,
                Day = x.Day,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DurationHours = x.DurationHours,


                Trainer = new TrainerDTO
                {
                    Id = x.Trainer.Id,
                    FullName = x.Trainer.FullName,
                    PhoneNumber = x.Trainer.PhoneNumber,
                    Salary = x.Trainer.Salary,
                    HireDate = x.Trainer.HireDate,
                }
            });
        }

        #endregion

    }
}
