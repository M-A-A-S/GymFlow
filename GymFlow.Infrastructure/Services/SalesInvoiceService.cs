using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Entities;
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
    public class SalesInvoiceService : ISalesInvoiceService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SalesInvoiceService> _logger;
        private readonly IInventoryService _inventoryService;

        #endregion

        #region ========================= Constructors =========================
        public SalesInvoiceService(
            IAppDbContext appDbContext,
            ILogger<SalesInvoiceService> logger,
            IInventoryService inventoryService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _inventoryService = inventoryService;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(SalesInvoiceDTO dto)
        {
            try
            {
                var validationResult = await ValidateSalesInvoiceDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();
                entity.Status = InvoiceStatus.Unpaid;
                entity.InvoiceNo = await GenerateInvoiceNumber();

                entity.Details = 
                    dto.Details
                    .Select(x => x.ToEntity())
                    .ToList();

                entity.Payments = 
                    dto.Payments?
                    .Select(x => x.ToEntity())
                    .ToList()
                    ??
                    new List<SalesPayment>();

                entity.CalculateTotal();
                UpdateInvoiceStatus(entity);

                var stockResult =
                    await _inventoryService.DecreaseStockAsync(
                        GetStockMovements(entity.Details));

                if (!stockResult.IsSuccess)
                {
                    return Result<int>.Failure(stockResult.Code, stockResult.StatusCode);
                }


                _appDbContext.SalesInvoices.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SalesInvoiceService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<SalesInvoiceDTO>>> GetAllAsync()
        {
            try
            {
                var SalesInvoices = await _appDbContext.SalesInvoices
                    .Include(x => x.Member)
                    .Include(x => x.Details)
                    .Include(x => x.Payments)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();

                await LoadItemDetails(SalesInvoices.SelectMany(x => x.Details).ToList());


                List <SalesInvoiceDTO> result = new();

                foreach (var item in SalesInvoices)
                {
                    var dto = item.ToDTO();
                    //dto.Details = item.Details.Select(x => x.ToDTO()).ToList();
                    foreach (var detail in  item.Details)
                    {
                        var detailDTO = detail.ToDTO();
                        detailDTO.Product = detail.Product.ToDTO();
                        detailDTO.SubscriptionType = detail.SubscriptionType.ToDTO();
                        dto.Details.Add(detailDTO);
                    }
                    dto.Payments = item.Payments.Select(x => x.ToDTO()).ToList();
                    if (item.Member != null)
                    {
                        dto.Member = item.Member.ToDTO();
                    }
                    result.Add(dto);
                }

                return Result<IEnumerable<SalesInvoiceDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SalesInvoiceService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<SalesInvoiceDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<SalesInvoiceDTO>> GetByIdAsync(int id)
        {
            try
            {
                var SalesInvoice = await _appDbContext.SalesInvoices
                    .Include(x => x.Member)
                    .Include(x => x.Details)
                    .Include(x => x.Payments)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (SalesInvoice == null)
                {
                    return Result<SalesInvoiceDTO>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }

                await LoadItemDetails(SalesInvoice.Details.ToList());


                var result = SalesInvoice.ToDTO();
                //result.Details = SalesInvoice.Details.Select(x => x.ToDTO()).ToList();
                foreach (var detail in SalesInvoice.Details)
                {
                    var detailDTO = detail.ToDTO();
                    detailDTO.Product = detail.Product.ToDTO();
                    detailDTO.SubscriptionType = detail.SubscriptionType.ToDTO();
                    result.Details.Add(detailDTO);
                }
                result.Payments = SalesInvoice.Payments.Select(x => x.ToDTO()).ToList();

                if (SalesInvoice.Member is not null)
                {
                    result.Member = SalesInvoice.Member.ToDTO();
                }

                return Result<SalesInvoiceDTO>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(SalesInvoiceService),
                   nameof(GetByIdAsync));

                return Result<SalesInvoiceDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<SalesInvoiceSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.SalesInvoices
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.InvoiceNo.Contains(search) ||
                    (x.Notes != null && x.Notes.Contains(search)) ||
                    (x.Member != null && x.Member.FullName.Contains(search)));
            }

            var SalesInvoices = await query
                .OrderByDescending(x => x.InvoiceDate)
                .Take(20)
                .Select(x => new SalesInvoiceSearchDTO
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    NetAmount = x.NetAmount,
                    PaidAmount = x.PaidAmount,
                    RemainingBalance = x.RemainingBalance,
                    Status = x.Status,
                    
                })
                .ToListAsync();

            return Result<IEnumerable<SalesInvoiceSearchDTO>>.Success(SalesInvoices);
        }

        public async Task<Result<SalesInvoiceAddUpdateDTO>> GetSalesInvoiceAddUpdateDTO(int? id = null)
        {
            var DTO = new SalesInvoiceAddUpdateDTO();

            if (id.HasValue)
            {
                var SalesInvoice = await _appDbContext.SalesInvoices
                    .Include(x => x.Member)
                    .Include(x => x.Details)
                    .Include(x => x.Payments)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (SalesInvoice is null)
                {
                    return Result<SalesInvoiceAddUpdateDTO>.Failure(
                        ResultCodes.NotFound);
                }

                await LoadItemDetails(SalesInvoice.Details.ToList());

                DTO.SalesInvoice = SalesInvoice.ToDTO();

                //DTO.SalesInvoice.Details = SalesInvoice.Details
                //    .Select(x => x.ToDTO()).ToList();
                foreach (var detail in SalesInvoice.Details)
                {
                    var detailDTO = detail.ToDTO();
                    detailDTO.Product = detail.Product.ToDTO();
                    detailDTO.SubscriptionType = detail.SubscriptionType.ToDTO();
                    DTO.SalesInvoice.Details.Add(detailDTO);
                }

                DTO.SalesInvoice.Payments = SalesInvoice.Payments
                    .Select(x => x.ToDTO()).ToList();

                if (SalesInvoice.Member is not null)
                {
                    DTO.SalesInvoice.Member = SalesInvoice.Member.ToDTO();
                }
            }

            DTO.Members = await _appDbContext.Members
                .Select(x => new MemberSearchDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync();

            DTO.Products = await _appDbContext.Products
                .Select(x => new ProductSearchDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    PurchasePrice = x.PurchasePrice,
                    SalePrice = x.SalePrice,
                }).ToListAsync();

            DTO.SubscriptionTypes = await _appDbContext.SubscriptionTypes
                .Select(x => new SubscriptionTypeSearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    DurationDays = x.DurationDays,
                    Price = x.Price,
                }).ToListAsync();

            DTO.Categories = await _appDbContext.Categories
                .Select(x => new CategorySearchDTO
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr
                }).ToListAsync();

            return Result<SalesInvoiceAddUpdateDTO>.Success(DTO);

        }

        #endregion

        #region ========================= Cancel =========================
        public async Task<Result<bool>> CancelAsync(int id, string? reason = null)
        {
            try
            {
                var invoice =
                    await _appDbContext.SalesInvoices
                    .Include(x => x.Details)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (invoice == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        HttpStatusCodes.NotFound);
                }

                if (invoice.Status == InvoiceStatus.Cancelled)
                {
                    return Result<bool>.Failure(
                        ResultCodes.AlreadyCancelled,
                        HttpStatusCodes.BadRequest);
                }

                var restoreStock =
                    await _inventoryService.IncreaseStockAsync(GetStockMovements(invoice.Details));

                if (!restoreStock.IsSuccess)
                {
                    return Result<bool>.Failure(
                        restoreStock.Code,
                        restoreStock.StatusCode);
                }

                invoice.Status =
                    InvoiceStatus.Cancelled;

                if (reason is not null)
                {
                    invoice.Notes =
                    $"{invoice.Notes} | Cancel reason: {reason}";
                }             

                invoice.UpdatedAt =
                    DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();

                return Result<bool>.Success(
                    true,
                    ResultCodes.UpdatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method : {Method}",
                    nameof(SalesInvoiceService),
                    nameof(CancelAsync));


                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }


        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, SalesInvoiceDTO dto)
        {

            try
            {
                var validationResult = await ValidateSalesInvoiceDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                //var entity = dto.ToEntity();

                var SalesInvoice = await _appDbContext.SalesInvoices
                    .Include(x => x.Details)
                    .Include(x => x.Payments)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (SalesInvoice == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, HttpStatusCodes.NotFound);
                }

                if (SalesInvoice.Status != InvoiceStatus.Draft)
                {
                    return Result<bool>.Failure(
                        ResultCodes.CannotEditPostedInvoice,
                        HttpStatusCodes.BadRequest);
                }

                SalesInvoice.InvoiceDate = dto.InvoiceDate;
                SalesInvoice.MemberId = dto.MemberId;
                SalesInvoice.Notes = dto.Notes;
                SalesInvoice.UpdatedAt = DateTime.UtcNow;


                var restoreStock =
                    await _inventoryService.IncreaseStockAsync(GetStockMovements(SalesInvoice.Details));

                if (!restoreStock.IsSuccess)
                {
                    return Result<bool>.Failure(
                        restoreStock.Code,
                        restoreStock.StatusCode);
                }

                // Remove old details
                _appDbContext.SalesInvoiceDetails.RemoveRange(SalesInvoice.Details);
                _appDbContext.SalesPayments.RemoveRange(SalesInvoice.Payments);

                // Add new details
                SalesInvoice.Details = dto.Details?.Select(x => x.ToEntity()).ToList() ?? new List<SalesInvoiceDetail>();

                // Add new payments
                SalesInvoice.Payments = dto.Payments?.Select(x => x.ToEntity()).ToList() ?? new List<SalesPayment>();

                var decreaseStock =
                    await _inventoryService.DecreaseStockAsync(GetStockMovements(SalesInvoice.Details));

                if (!decreaseStock.IsSuccess)
                {
                    return Result<bool>.Failure(
                        decreaseStock.Code,
                        decreaseStock.StatusCode);
                }

                SalesInvoice.CalculateTotal();
                SalesInvoice.Status = InvoiceStatus.Draft;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SalesInvoiceService),
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
                var SalesInvoice = await _appDbContext.SalesInvoices
                    .Include(x => x.Details)
                    .Include(x => x.Payments)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (SalesInvoice == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        HttpStatusCodes.NotFound);
                }

                var restoreStock =
                    await _inventoryService.IncreaseStockAsync(GetStockMovements(SalesInvoice.Details));

                if (!restoreStock.IsSuccess)
                {
                    return Result<bool>.Failure(
                        restoreStock.Code,
                        restoreStock.StatusCode);
                }

                var now = DateTime.UtcNow;
                foreach (var item in SalesInvoice.Details)
                {
                    item.IsDeleted = true;
                    item.UpdatedAt = now;
                    item.DeletedAt = now;
                }

                foreach (var item in SalesInvoice.Payments)
                {
                    item.IsDeleted = true;
                    item.UpdatedAt = now;
                    item.DeletedAt = now;
                }

                SalesInvoice.IsDeleted = true;
                SalesInvoice.UpdatedAt = now;
                SalesInvoice.DeletedAt = now;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(SalesInvoiceService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidateSalesInvoiceDTO(
            SalesInvoiceDTO DTO,
            int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    HttpStatusCodes.BadRequest);
            }

            // Details required
            if (DTO.Details is null || !DTO.Details.Any())
            {
                return Result<bool>.Failure(
                    ResultCodes.SalesDetailsRequired,
                    HttpStatusCodes.BadRequest);
            }

            // Duplicate Sale Items
            if (DTO.Details
                .GroupBy(x => new
                {
                    x.ItemType,
                    x.ItemId,
                })
                .Any(g => g.Count() > 1))
            {
                return Result<bool>.Failure(
                    ResultCodes.DuplicateItemInInvoice,
                    HttpStatusCodes.BadRequest);
            }

            // Quantity validation
            if (DTO.Details.Any(x => x.Quantity <= 0))
            {
                return Result<bool>.Failure(
                        ResultCodes.InvalidQuantity,
                        HttpStatusCodes.BadRequest);
            }

            // Price validation
            if (DTO.Details.Any(x => x.UnitPrice <= 0))
            {
                return Result<bool>.Failure(
                        ResultCodes.InvalidUnitPrice,
                        HttpStatusCodes.BadRequest);
            }

            // Member validation
            if (DTO.MemberId.HasValue)
            {
                bool memberExists =
                await _appDbContext.Members
                .AnyAsync(x => x.Id == DTO.MemberId);

                if (!memberExists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.MemberNotFound,
                        HttpStatusCodes.NotFound);
                }
            }


            // Validate sold items
            var productIds = DTO.Details
                .Where(x => x.ItemType == SaleItemType.Product)
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            var subscriptionIds = DTO.Details
                .Where(x => x.ItemType == SaleItemType.Subscription)
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            // Products
            if (productIds.Any())
            {
                var existingProductIds =
                    await _appDbContext.Products
                    .Where(x => productIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync();

                if (existingProductIds.Count != productIds.Count)
                {
                    return Result<bool>.Failure(
                        ResultCodes.ProductNotFound,
                        HttpStatusCodes.NotFound);
                }
            }

            // Subscription Types
            if (subscriptionIds.Any())
            {
                var existingSubscriptionIds =
                    await _appDbContext.SubscriptionTypes
                    .Where(x => subscriptionIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync();

                if (existingSubscriptionIds.Count != subscriptionIds.Count)
                {
                    return Result<bool>.Failure(
                        ResultCodes.SubscriptionPlanNotFound);
                }
            }

            //// Validate sold items
            //foreach (var detail in DTO.Details)
            //{
            //    switch(detail.ItemType)
            //    {
            //        case SaleItemType.Product:
            //            bool productExists =
            //                await _appDbContext.Products
            //                .AnyAsync(x => x.Id == detail.ItemId);

            //            if (!productExists)
            //            {
            //                return Result<bool>.Failure(
            //                        ResultCodes.ProductNotFound,
            //                        HttpStatusCodes.NotFound);
            //            }
            //            break;

            //        case SaleItemType.Subscription:
            //            bool subscriptionExists =
            //                await _appDbContext.SubscriptionTypes
            //                .AnyAsync(x => x.Id == detail.ItemId);

            //            if (!subscriptionExists)
            //            {
            //                return Result<bool>.Failure(
            //                        ResultCodes.SubscriptionPlanNotFound,
            //                        HttpStatusCodes.NotFound);
            //            }
            //            break;

            //        //case SaleItemType.Service:

            //        //    var serviceExists =
            //        //        await _appDbContext.Services
            //        //        .AnyAsync(x => x.Id == detail.ItemId);



            //        //    if (!serviceExists)
            //        //    {
            //        //        return Result<bool>.Failure(
            //        //            ResultCodes.ServiceNotFound,
            //        //            HttpStatusCodes.NotFound);
            //        //    }

            //        //    break;

            //        default:

            //            return Result<bool>.Failure(
            //                ResultCodes.InvalidSaleItemType,
            //                HttpStatusCodes.BadRequest);

            //    }
            //}

            // Payment validation
            if (DTO.Payments != null && DTO.Payments.Any())
            {

                if (DTO.Payments.Any(x => x.Amount <= 0))
                {
                    return Result<bool>.Failure(
                            ResultCodes.InvalidPaymentAmount,
                            HttpStatusCodes.BadRequest);
                }

                if (DTO.Payments.Any(x => x.PaymentDate == default))
                {
                    return Result<bool>.Failure(
                            ResultCodes.InvalidPaymentDate,
                            HttpStatusCodes.BadRequest);
                }

                // prevent paying more than the invoice total
                var invoiceTotal = DTO.Details.Sum(x => x.Quantity * x.UnitPrice);
                var paid = DTO.Payments.Sum(x => x.Amount);

                if (paid > invoiceTotal)
                {
                    return Result<bool>.Failure(
                        ResultCodes.PaymentExceedsInvoiceTotal,
                        HttpStatusCodes.BadRequest);
                }

            }

            return Result<bool>.Success(true);

        }

        private async Task<string> GenerateInvoiceNumber()
        {
            var now = DateTime.UtcNow;

            var prefix = $"SAL-{now:yyyy-MM}-";

            var lastInvoiceNo = await _appDbContext.SalesInvoices
                .Where(x => x.InvoiceNo.StartsWith(prefix))
                .OrderByDescending(x => x.Id)
                .Select(x => x.InvoiceNo)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(lastInvoiceNo))
            {
                return $"{prefix}000001";
            }

            var numberPart = lastInvoiceNo.Replace(prefix, "");

            if (int.TryParse(numberPart, out int number))
            {
                number++;

                return $"{prefix}{number:D6}";
            }

            // fallback in case old data has invalid format
            return $"{prefix}{DateTime.UtcNow:MMddHHmmss}";
        }

        private void UpdateInvoiceStatus(SalesInvoice invoice)
        {
            if (invoice.Status == InvoiceStatus.Cancelled)
            {
                return;
            }

            if (invoice.Status == InvoiceStatus.Draft)
            {
                return;
            }

            invoice.PaidAmount = invoice.Payments
                .Where(x => !x.IsVoided)
                .Sum(x => x.Amount);

            invoice.RemainingBalance = invoice.NetAmount - invoice.PaidAmount;

            if (invoice.PaidAmount <= 0)
            {
                invoice.Status = InvoiceStatus.Unpaid;
            }
            else if (invoice.RemainingBalance > 0)
            {
                invoice.Status = InvoiceStatus.Partial;
            }
            else
            {
                invoice.Status = InvoiceStatus.Paid;
            }
        }

        private async Task LoadItemDetails(List<SalesInvoiceDetail> details)
        {
            var productIds = details
                    .Where(x => x.ItemType == SaleItemType.Product)
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();

            var subscriptionIds = details
                .Where(x => x.ItemType == SaleItemType.Subscription)
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            var products =
                await _appDbContext.Products
                .AsNoTracking()
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var subscriptions =
                await _appDbContext.SubscriptionTypes
                .AsNoTracking()
                .Where(x => subscriptionIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var detail in details)
            {
                if (detail.ItemType == SaleItemType.Product)
                {
                    detail.Product = products.GetValueOrDefault(detail.ItemId);
                }

                if (detail.ItemType == SaleItemType.Subscription)
                {
                    detail.SubscriptionType = subscriptions.GetValueOrDefault(detail.ItemId);
                }
            }

        }

        private static List<StockMovementDTO> GetStockMovements(IEnumerable<SalesInvoiceDetail> details)
        {
            return details
                .Where(x => x.ItemType == SaleItemType.Product)
                .Select(x => new StockMovementDTO
                {
                    ProductId = x.ItemId,
                    Quantity = x.Quantity
                }).ToList();
        }

        #endregion

    }
}
