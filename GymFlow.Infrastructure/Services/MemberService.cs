using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Extensions;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<MemberService> _logger;

        public MemberService(
            IAppDbContext appDbContext,
            ILogger<MemberService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #region Add
        public async Task<Result<int>> AddAsync(MemberDTO dto)
        {
            bool emailExists = 
                await _appDbContext.Members
                .AnyAsync(m => m.Email == dto.Email && m.Email != null);

            if (emailExists)
            {
                return Result<int>.Failure(
                    ResultCodes.EmailExists, 409);
            }

            bool phoneNumberExists = 
                await _appDbContext.Members
                .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber);

            if (phoneNumberExists)
            {
                return Result<int>.Failure(
                    ResultCodes.PhoneExists, 409);
            }

            var entity = dto.ToEntity();

            try
            {
                _appDbContext.Members.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region Get
        public async Task<Result<IEnumerable<MemberDTO>>> GetAllAsync()
        {
            try
            {
                var members = await _appDbContext.Members
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<MemberDTO>>.Success(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<MemberDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<MemberDTO>> GetByIdAsync(int id)
        {
            try
            {
                var member = await _appDbContext.Members
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (member == null)
                {
                    return Result<MemberDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<MemberDTO>.Success(member.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberService),
                   nameof(GetByIdAsync));

                return Result<MemberDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        #endregion

        #region Update
        public async Task<Result<bool>> UpdateAsync(int id, MemberDTO dto)
        {

            try
            {
                var member = _appDbContext.Members.FirstOrDefault(x => x.Id == id);

                if (member == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                bool emailExists = await _appDbContext.Members
                    .AnyAsync(m => m.Email == dto.Email && m.Id != id && m.Email != null);

                if (emailExists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.EmailExists,
                        409);
                }

                bool phoneExists = await _appDbContext.Members
                    .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber && m.Id != id);

                if (phoneExists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.PhoneExists,
                        409);
                }

                member.FullName = dto.FullName;
                member.Email = dto.Email;
                member.PhoneNumber = dto.PhoneNumber;
                member.Gender = dto.Gender;
                member.BirthDate = dto.BirthDate;
                member.Status = dto.Status ?? MemberStatus.Active;
                member.Address = dto.Address;
                member.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberService),
                    nameof(UpdateAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError, 
                    500, "An unexpected error occurred.");
            }
        }
        #endregion

        #region Delete
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var member = await _appDbContext.Members.FirstOrDefaultAsync(x => x.Id == id);

                if (member == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                member.IsDeleted = true;
                member.UpdatedAt = DateTime.UtcNow;
                member.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

    }
}
