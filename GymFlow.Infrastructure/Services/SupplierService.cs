using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Supplier;
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
    public class SupplierService : ISupplierService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SupplierService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public SupplierService(
            IAppDbContext appDbContext,
            ILogger<SupplierService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(SupplierDTO dto)
        {

            try
            {
                var validationResult = await ValidateSupplierDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                _appDbContext.Suppliers.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SupplierService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<SupplierDTO>>> GetAllAsync()
        {
            try
            {
                var suppliers = await _appDbContext.Suppliers
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<SupplierDTO>>.Success(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SupplierService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<SupplierDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<SupplierDTO>> GetByIdAsync(int id)
        {
            try
            {
                var supplier = await _appDbContext.Suppliers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (supplier == null)
                {
                    return Result<SupplierDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<SupplierDTO>.Success(supplier.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SupplierService),
                   nameof(GetByIdAsync));

                return Result<SupplierDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<SupplierSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.Suppliers
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FullName.Contains(search) ||
                    x.PhoneNumber.Contains(search));
            }

            var suppliers = await query
                .Take(20)
                .Select(x => new SupplierSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                })
                .ToListAsync();

            return Result<IEnumerable<SupplierSearchDTO>>.Success(suppliers);
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, SupplierDTO dto)
        {

            try
            {
                var validationResult = await ValidateSupplierDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                var supplier = _appDbContext.Suppliers.FirstOrDefault(x => x.Id == id);

                if (supplier == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                supplier.FullName = dto.FullName;
                supplier.PhoneNumber = dto.PhoneNumber;
                supplier.Address = dto.Address;
                supplier.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SupplierService),
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
                var supplier = await _appDbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == id);

                if (supplier == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                supplier.IsDeleted = true;
                supplier.UpdatedAt = DateTime.UtcNow;
                supplier.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SupplierService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateSupplierDTO(SupplierDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            bool phoneNumberExists =
                await _appDbContext.Suppliers
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
