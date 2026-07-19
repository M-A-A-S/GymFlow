using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Trainer;
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
    public class TrainerService : ITrainerService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<TrainerService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public TrainerService(
            IAppDbContext appDbContext,
            ILogger<TrainerService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(TrainerDTO dto)
        {    

            try
            {
                var validationResult = await ValidateTrainerDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                _appDbContext.Trainers.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<TrainerDTO>>> GetAllAsync()
        {
            try
            {
                var trainers = await _appDbContext.Trainers
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<TrainerDTO>>.Success(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<TrainerDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<TrainerDTO>> GetByIdAsync(int id)
        {
            try
            {
                var trainer = await _appDbContext.Trainers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (trainer == null)
                {
                    return Result<TrainerDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<TrainerDTO>.Success(trainer.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(TrainerService),
                   nameof(GetByIdAsync));

                return Result<TrainerDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<TrainerSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.Trainers
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FullName.Contains(search) ||
                    x.PhoneNumber.Contains(search));
            }

            var trainers = await query
                .Take(20)
                .Select(x => new TrainerSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                })
                .ToListAsync();

            return Result<IEnumerable<TrainerSearchDTO>>.Success(trainers);
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, TrainerDTO dto)
        {

            try
            {
                var validationResult = await ValidateTrainerDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                var trainer = _appDbContext.Trainers.FirstOrDefault(x => x.Id == id);

                if (trainer == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                trainer.FullName = dto.FullName;
                trainer.PhoneNumber = dto.PhoneNumber;
                trainer.Salary = dto.Salary;
                trainer.HireDate = dto.HireDate;
                trainer.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(TrainerService),
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
                var trainer = await _appDbContext.Trainers.FirstOrDefaultAsync(x => x.Id == id);

                if (trainer == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                trainer.IsDeleted = true;
                trainer.UpdatedAt = DateTime.UtcNow;
                trainer.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(TrainerService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateTrainerDTO(TrainerDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            if (DTO.Salary < 0)
            {
                return Result<bool>.Failure(
                    ResultCodes.ValueCannotBeNegative,
                    400);
            }

            bool phoneNumberExists =
                await _appDbContext.Trainers
                .AnyAsync(m => m.PhoneNumber == DTO.PhoneNumber && (excludedId == null || m.Id != excludedId));

            if (phoneNumberExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.PhoneExists, 409);
            }

            return Result<bool>.Success(true);

        }

        #endregion

    }
}
