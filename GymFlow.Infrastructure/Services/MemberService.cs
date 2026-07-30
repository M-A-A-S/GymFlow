using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<MemberService> _logger;
        private readonly IMemoryCache _cache;

        #endregion

        #region ========================= Constructors =========================
        public MemberService(
            IAppDbContext appDbContext,
            ILogger<MemberService> logger,
            IMemoryCache cache)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _cache = cache;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(MemberDTO dto)
        {
            bool emailExists = 
                await _appDbContext.Members
                .AnyAsync(m => m.Email == dto.Email && m.Email != null);

            if (emailExists)
            {
                return Result<int>.Failure(
                    ResultCodes.EmailExists, HttpStatusCodes.Conflict);
            }

            bool phoneNumberExists = 
                await _appDbContext.Members
                .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber);

            if (phoneNumberExists)
            {
                return Result<int>.Failure(
                    ResultCodes.PhoneExists, HttpStatusCodes.Conflict);
            }

            var entity = dto.ToEntity();

            try
            {
                _appDbContext.Members.Add(entity);
                await _appDbContext.SaveChangesAsync();
                _cache.Remove(CacheKeys.MembersSelect);
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<MemberDTO>>> GetAllAsync(MemberFilterDTO filter)
        {
            try
            {
                //var members = await _appDbContext.Members
                //.Select(m => m.ToDTO())
                //.AsNoTracking()
                //.ToListAsync();

                var query = 
                    _appDbContext.Members
                    .AsNoTracking();

                // Search
                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                        x.FullName.Contains(filter.Search) ||
                        x.PhoneNumber.Contains(filter.Search)
                        );
                }

                // Gender filter
                if (filter.Gender.HasValue)
                {
                    query =
                        query.Where(x => x.Gender == filter.Gender);
                }

                // Status filter
                if (filter.Status.HasValue)
                {
                    query =
                        query.Where(x => x.Status == filter.Status);
                }

                // Date filter
                if (filter.RegisterDateFrom.HasValue)
                {
                    query =
                        query.Where(x => x.RegisterDate >= filter.RegisterDateFrom);
                }

                if (filter.RegisterDateTo.HasValue)
                {
                    query =
                        query.Where(x => x.RegisterDate <= filter.RegisterDateTo);
                }

                //query = query.OrderByDescending(x => x.Id);
                query = query.OrderByProperty(filter.SortBy, filter.Descending);

                var pagedResult = await query.ToPagedListAsync(filter.PageNumber, filter.PageSize);

                var result = new PagedResult<MemberDTO>
                {
                    Items = pagedResult.Items.Select(x => x.ToDTO()),
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount,
                    TotalPages = pagedResult.TotalPages,
                };


                return Result<PagedResult<MemberDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(MemberService),
                   nameof(GetAllAsync));

                return Result<PagedResult<MemberDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
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
                    return Result<MemberDTO>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<MemberSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.Members
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FullName.Contains(search) ||
                    x.PhoneNumber.Contains(search));
            }

            var members = await query
                .Take(20)
                .Select(x => new MemberSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                })
                .ToListAsync();

            return Result<IEnumerable<MemberSearchDTO>>.Success(members);
        }

        public async Task<Result<IEnumerable<MemberSearchDTO>>> GetForSelectAsync()
        {
            try
            {
                if (_cache.TryGetValue(
                    CacheKeys.MembersSelect,
                    out IEnumerable<MemberSearchDTO>? members))
                {
                    return Result<IEnumerable<MemberSearchDTO>>
                        .Success(members);
                }


                members = await _appDbContext.Members
                    .AsNoTracking()
                    .OrderBy(x => x.FullName)
                    .Select(x => new MemberSearchDTO
                    {
                        Id = x.Id,
                        FullName = x.FullName
                    })
                    .ToListAsync();


                _cache.Set(
                    CacheKeys.MembersSelect,
                    members,
                    new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                    });


                return Result<IEnumerable<MemberSearchDTO>>
                    .Success(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading members for select");

                return Result<IEnumerable<MemberSearchDTO>>
                    .Failure(
                        ResultCodes.UnexpectedError,
                        HttpStatusCodes.InternalServerError,
                        "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, MemberDTO dto)
        {

            try
            {
                var member = _appDbContext.Members.FirstOrDefault(x => x.Id == id);

                if (member == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }

                bool emailExists = await _appDbContext.Members
                    .AnyAsync(m => m.Email == dto.Email && m.Id != id && m.Email != null);

                if (emailExists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.EmailExists,
                        HttpStatusCodes.Conflict);
                }

                bool phoneExists = await _appDbContext.Members
                    .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber && m.Id != id);

                if (phoneExists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.PhoneExists,
                        HttpStatusCodes.Conflict);
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
                _cache.Remove(CacheKeys.MembersSelect);
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
                    HttpStatusCodes.InternalServerError, "An unexpected error occurred.");
            }
        }
        #endregion

        #region ========================= Delete =========================
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var member = await _appDbContext.Members.FirstOrDefaultAsync(x => x.Id == id);

                if (member == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        HttpStatusCodes.NotFound);
                }

                member.IsDeleted = true;
                member.UpdatedAt = DateTime.UtcNow;
                member.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                _cache.Remove(CacheKeys.MembersSelect);
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
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }

        }
        #endregion

    }
}
