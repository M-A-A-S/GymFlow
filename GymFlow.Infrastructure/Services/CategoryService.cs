using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.Trainer;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        #endregion

        #region ========================= Constructors =========================
        public CategoryService(
            IAppDbContext appDbContext,
            ILogger<CategoryService> logger,
            IFileService fileService,
            IMemoryCache cache)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _fileService = fileService;
            _cache = cache;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(CategoryDTO dto)
        {

            try
            {
                var validationResult = await ValidateCategoryDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var imageResult = await _fileService.SaveAsync(
                    dto.Image,
                    dto.ImageUrl,
                    Constants.CategoriesFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();

                entity.ImageUrl = imageResult.Data;

                _appDbContext.Categories.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(CategoryService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync()
        {
            try
            {
                var categorys = await _appDbContext.Categories
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<CategoryDTO>>.Success(categorys);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(CategoryService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<CategoryDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<CategoryDTO>>> GetAllAsync(CategoryFilterDTO filter)
        {
            try
            {
                var query = _appDbContext.Categories
                .AsNoTracking();

                query = ApplyFilters(query, filter);

                query = ApplySorting(query, filter);

                var dtoQuery = ProjectToDTO(query);

                var pagedResult = await dtoQuery.ToPagedListAsync(
                    filter.PageNumber,
                    filter.PageSize);

                return Result<PagedResult<CategoryDTO>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(CategoryService),
                   nameof(GetAllAsync));

                return Result<PagedResult<CategoryDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<CategoryDTO>> GetByIdAsync(int id)
        {
            try
            {
                var category = await _appDbContext.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return Result<CategoryDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<CategoryDTO>.Success(category.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(CategoryService),
                   nameof(GetByIdAsync));

                return Result<CategoryDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<CategorySearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.Categories
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.NameEn.Contains(search) ||
                    x.NameAr.Contains(search));
            }

            var categories = await query
                //.Take(20)
                .Select(x => new CategorySearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                })
                .ToListAsync();

            return Result<IEnumerable<CategorySearchDTO>>.Success(categories);
        }

        public async Task<Result<IEnumerable<CategorySearchDTO>>> GetForSelectAsync()
        {
            try
            {
                if (_cache.TryGetValue(
                    CacheKeys.CategoriesSelect,
                    out IEnumerable<CategorySearchDTO>? categories))
                {
                    return Result<IEnumerable<CategorySearchDTO>>
                        .Success(categories);
                }


                categories = await _appDbContext.Categories
                    .AsNoTracking()
                    .OrderBy(x => x.NameEn)
                    .Select(x => new CategorySearchDTO
                    {
                        Id = x.Id,
                        NameEn = x.NameEn,
                        NameAr = x.NameAr,
                    })
                    .ToListAsync();


                _cache.Set(
                    CacheKeys.CategoriesSelect,
                    categories,
                    new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                    });


                return Result<IEnumerable<CategorySearchDTO>>
                    .Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading categories for select");

                return Result<IEnumerable<CategorySearchDTO>>
                    .Failure(
                        ResultCodes.UnexpectedError,
                        HttpStatusCodes.InternalServerError,
                        "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, CategoryDTO dto)
        {

            try
            {
                var validationResult = await ValidateCategoryDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                //var entity = dto.ToEntity();

                var category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);

                if (category == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                var imageResult = await _fileService.ReplaceAsync(
                    dto.Image,
                    dto.ImageUrl,
                    category.ImageUrl,
                    Constants.CategoriesFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        imageResult.Code,
                        imageResult.StatusCode);
                }

                category.ImageUrl = imageResult.Data;

                category.NameEn = dto.NameEn;
                category.NameAr = dto.NameAr;
                category.DescriptionEn = dto.DescriptionEn;
                category.DescriptionAr = dto.DescriptionAr;
                category.IsActive = dto.IsActive;
                category.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(CategoryService),
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
                var category = await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                var deleteFileResult = await _fileService.DeleteAsync(
                    category.ImageUrl,
                    Constants.CategoriesFolder);

                if (!deleteFileResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        deleteFileResult.Code,
                        deleteFileResult.StatusCode);
                }

                category.IsDeleted = true;
                category.UpdatedAt = DateTime.UtcNow;
                category.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(CategoryService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateCategoryDTO(CategoryDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            return Result<bool>.Success(true);

        }

        private IQueryable<Category> ApplyFilters(
            IQueryable<Category> query,
            CategoryFilterDTO filter)
        {
            // ========================== Search ==========================
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.NameEn.Contains(filter.Search) ||
                    x.NameAr.Contains(filter.Search)
                    );
            }

            // ========================== IsActive ==========================
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            return query;

        }

        private IQueryable<Category> ApplySorting(
            IQueryable<Category> query,
            CategoryFilterDTO filter)
        {
            bool desc = filter.Descending;
            bool isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            switch (filter.SortBy)
            {

                // ========================= Name ========================= 
                case "Name":
                    if (isArabic)
                    {
                        return desc
                            ? query.OrderByDescending(x => x.NameAr)
                            : query.OrderBy(x => x.NameAr);
                    }
                    else
                    {
                        return desc
                            ? query.OrderByDescending(x => x.NameEn)
                            : query.OrderBy(x => x.NameEn);
                    }


                // ========================= Entity Properties =========================
                default:
                    return query.OrderByProperty(filter.SortBy, desc);
            }

        }

        private IQueryable<CategoryDTO> ProjectToDTO(
            IQueryable<Category> query
            )
        {
            return query.Select(x => x.ToDTO());
        }

        #endregion

    }
}
