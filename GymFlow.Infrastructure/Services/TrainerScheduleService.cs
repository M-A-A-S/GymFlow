using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.DTOs.TrainerSchedule;
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
                trainerSchedule.Day = dto.Day;
                trainerSchedule.StartTime = dto.StartTime;
                trainerSchedule.EndTime = dto.EndTime;
                trainerSchedule.UpdatedAt = DateTime.UtcNow;

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

        #endregion

    }
}
