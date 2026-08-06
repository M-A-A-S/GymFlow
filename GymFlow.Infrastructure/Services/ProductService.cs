using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.SubscriptionType;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IFileService _fileService;

        #endregion

        #region ========================= Constructors =========================
        public ProductService(
            IAppDbContext appDbContext,
            ILogger<ProductService> logger,
            IFileService fileService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _fileService = fileService;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(ProductDTO dto)
        {

            try
            {
                var validationResult = await ValidateProductDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var imageResult = await _fileService.SaveAsync(
                    dto.Image,
                    dto.ImageUrl,
                    Constants.ProductsFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();
                entity.Code =  await GenerateProductCode();

                entity.ImageUrl = imageResult.Data;

                _appDbContext.Products.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(ProductService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            try
            {
                var products = await _appDbContext.Products.Include(x => x.Category)
                .Select(m => m.ToDTO())
                .AsNoTracking()
                .ToListAsync();

                return Result<IEnumerable<ProductDTO>>.Success(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(ProductService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<ProductDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PagedResult<ProductDTO>>> GetAllAsync(ProductFilterDTO filter)
        {
            try
            {
                var query = _appDbContext.Products
                .Include(x => x.Category)
                .AsNoTracking();

                query = ApplyFilters(query, filter);

                query = ApplySorting(query, filter);

                var dtoQuery = ProjectToDTO(query);

                var pagedResult = await dtoQuery.ToPagedListAsync(
                    filter.PageNumber,
                    filter.PageSize);

                return Result<PagedResult<ProductDTO>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(ProductService),
                   nameof(GetAllAsync));

                return Result<PagedResult<ProductDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<ProductDTO>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _appDbContext.Products
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    return Result<ProductDTO>.Failure(ResultCodes.NotFound, 404);
                }
                return Result<ProductDTO>.Success(product.ToDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(ProductService),
                   nameof(GetByIdAsync));

                return Result<ProductDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<ProductSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.Products
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.NameEn.Contains(search) ||
                    x.Code.Contains(search) ||
                    x.NameAr.Contains(search));
            }

            var products = await query
                //.Take(20)
                .Select(x => new ProductSearchDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                })
                .ToListAsync();

            return Result<IEnumerable<ProductSearchDTO>>.Success(products);
        }

        public async Task<Result<ProductAddUpdateDTO>> GetProductAddUpdateDTO(int? id = null)
        {
            var DTO = new ProductAddUpdateDTO();

            if (id.HasValue)
            {
                var product = await _appDbContext.Products
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id.Value);

                if (product is null)
                {
                    return Result<ProductAddUpdateDTO>.Failure(
                        ResultCodes.NotFound);
                }

                DTO.Product = product.ToDTO();
            }

            DTO.Categories = await _appDbContext.Categories
                .Select(x => new CategorySearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                })
                .ToListAsync();

            return Result<ProductAddUpdateDTO>.Success(DTO);

        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, ProductDTO dto)
        {

            try
            {
                var validationResult = await ValidateProductDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                //var entity = dto.ToEntity();

                var product = _appDbContext.Products.FirstOrDefault(x => x.Id == id);

                if (product == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                var imageResult = await _fileService.ReplaceAsync(
                    dto.Image,
                    dto.ImageUrl,
                    product.ImageUrl,
                    Constants.ProductsFolder);

                if (!imageResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        imageResult.Code,
                        imageResult.StatusCode);
                }

                product.ImageUrl = imageResult.Data;

                product.NameEn = dto.NameEn;
                product.NameAr = dto.NameAr;
                product.DescriptionEn = dto.DescriptionEn;
                product.DescriptionAr = dto.DescriptionAr;
                product.CategoryId = dto.CategoryId;
                product.PurchasePrice = dto.PurchasePrice.Value;
                product.SalePrice = dto.SalePrice.Value;
                product.Quantity = dto.Quantity.Value;
                product.ReorderLevel = dto.ReorderLevel.Value;
                product.UpdatedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(ProductService),
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
                var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        404);
                }

                var deleteFileResult = await _fileService.DeleteAsync(
                    product.ImageUrl,
                    Constants.ProductsFolder);

                if (!deleteFileResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        deleteFileResult.Code,
                        deleteFileResult.StatusCode);
                }

                product.IsDeleted = true;
                product.UpdatedAt = DateTime.UtcNow;
                product.DeletedAt = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(ProductService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateProductDTO(ProductDTO DTO, int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    400);
            }

            var codeExists = await _appDbContext.Products
                .AnyAsync(x =>
                    x.Code == DTO.Code &&
                    (!excludedId.HasValue || x.Id != excludedId.Value));

            if (codeExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidValue,
                    400);
            }

            var nameEnExists = await _appDbContext.Products
                .AnyAsync(x =>
                    x.NameEn == DTO.NameEn &&
                    (!excludedId.HasValue || x.Id != excludedId.Value));

            if (nameEnExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.NameEnAlreadyExists,
                    400);
            }

            var nameArExists = await _appDbContext.Products
                .AnyAsync(x =>
                    x.NameAr == DTO.NameAr &&
                    (!excludedId.HasValue || x.Id != excludedId.Value));

            if (nameArExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.NameArAlreadyExists,
                    400);
            }

            return Result<bool>.Success(true);

        }

        private async Task<string> GenerateProductCode()
        {
            var lastCode = await _appDbContext.Products
                .OrderByDescending(x => x.Id)
                .Select(x => x.Code)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(lastCode))
            {
                return "PRD-000001";
            }

            var numberPart = lastCode.Replace("PRD-", "");

            if (int.TryParse(numberPart, out int number))
            {
                number++;

                return $"PRD-{number:D6}";
            }

            return $"PRD-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        private IQueryable<Product> ApplyFilters(
            IQueryable<Product> query,
            ProductFilterDTO filter)
        {
            // ========================== Search ==========================
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.Code.Contains(filter.Search) || 
                    x.NameEn.Contains(filter.Search) || 
                    x.NameAr.Contains(filter.Search)
                    );
            }

            // ========================== Category ==========================
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryId == filter.CategoryId.Value);
            }


            // ========================== Purchase Price ==========================
            if (filter.MinPurchasePrice.HasValue)
            {
                query = query.Where(x =>
                    x.PurchasePrice >= filter.MinPurchasePrice.Value);
            }

            if (filter.MaxPurchasePrice.HasValue)
            {
                query = query.Where(x =>
                    x.PurchasePrice <= filter.MaxPurchasePrice.Value);
            }

            // ========================== Sale Price ==========================
            if (filter.MinSalePrice.HasValue)
            {
                query = query.Where(x =>
                    x.SalePrice >= filter.MinSalePrice.Value);
            }

            if (filter.MaxSalePrice.HasValue)
            {
                query = query.Where(x =>
                    x.SalePrice <= filter.MaxSalePrice.Value);
            }

            // ========================== Quantity ==========================
            if (filter.MinQuantity.HasValue)
            {
                query = query.Where(x =>
                    x.Quantity >= filter.MinQuantity.Value);
            }

            if (filter.MaxQuantity.HasValue)
            {
                query = query.Where(x =>
                    x.Quantity <= filter.MaxQuantity.Value);
            }

            // ========================== Reorder Level ==========================
            if (filter.MinReorderLevel.HasValue)
            {
                query = query.Where(x =>
                    x.ReorderLevel >= filter.MinReorderLevel.Value);
            }

            if (filter.MaxReorderLevel.HasValue)
            {
                query = query.Where(x =>
                    x.ReorderLevel <= filter.MaxReorderLevel.Value);
            }

            if (filter.LowStockOnly == true)
            {
                query = query.Where(x => x.Quantity <= x.ReorderLevel);
            }

            if (filter.OutOfStockOnly == true)
            {
                query = query.Where(x => x.Quantity == 0);
            }


            return query;

        }

        private IQueryable<Product> ApplySorting(
            IQueryable<Product> query,
            ProductFilterDTO filter)
        {
            bool desc = filter.Descending;
            bool isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            switch (filter.SortBy)
            {
                // ========================= Category ========================= 
                case "Category":
                    if (isArabic)
                    {
                        return desc
                            ? query.OrderByDescending(x => x.Category.NameAr)
                            : query.OrderBy(x => x.Category.NameAr);
                    }
                    else
                    {
                        return desc
                            ? query.OrderByDescending(x => x.Category.NameEn)
                            : query.OrderBy(x => x.Category.NameEn);
                    }

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

        private IQueryable<ProductDTO> ProjectToDTO(
            IQueryable<Product> query
            )
        {
            return query.Select(x => new ProductDTO
            {
                Id = x.Id,
                Code = x.Code,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                ImageUrl = x.ImageUrl,
                CategoryId = x.CategoryId,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.SalePrice,
                Quantity = x.Quantity,
                ReorderLevel = x.ReorderLevel,


                Category = x.Category == null ? null : new CategoryDTO
                {
                    Id = x.Category.Id,
                    NameEn = x.Category.NameEn,
                    NameAr = x.Category.NameAr,
                    DescriptionEn = x.Category.DescriptionEn,
                    DescriptionAr = x.Category.DescriptionAr,
                    ImageUrl = x.Category.ImageUrl,
                }
            });
        }

        #endregion

    }
}
