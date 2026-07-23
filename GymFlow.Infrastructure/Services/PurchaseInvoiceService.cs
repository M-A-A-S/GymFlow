using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.PurchaseInvoice;
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
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<PurchaseInvoiceService> _logger;

        #endregion

        #region ========================= Constructors =========================
        public PurchaseInvoiceService(
            IAppDbContext appDbContext,
            ILogger<PurchaseInvoiceService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Add =========================
        public async Task<Result<int>> AddAsync(PurchaseInvoiceDTO dto)
        {

            try
            {
                var validationResult = await ValidatePurchaseInvoiceDTO(dto);

                if (!validationResult.IsSuccess)
                {
                    return Result<int>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                var entity = dto.ToEntity();
                //if (string.IsNullOrWhiteSpace(entity.InvoiceNo))
                //{
                //    entity.InvoiceNo = await GenerateInvoiceNumber();
                //}
                entity.InvoiceNo = await GenerateInvoiceNumber();
                entity.PurchaseDetails = dto.PurchaseDetails.Select(x => x.ToEntity()).ToList();
                entity.PurchasePayments = dto.PurchasePayments.Select(x => x.ToEntity()).ToList();
                entity.CalculateTotal();
                entity.UpdatePaymentStatus();


                _appDbContext.PurchaseInvoices.Add(entity);
                await _appDbContext.SaveChangesAsync();
                return Result<int>.Success(entity.Id, ResultCodes.CreatedSuccessfully);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(PurchaseInvoiceService),
                   nameof(AddAsync));

                return Result<int>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");

            }
        }
        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<PurchaseInvoiceDTO>>> GetAllAsync()
        {
            try
            {
                var purchaseInvoices = await _appDbContext.PurchaseInvoices
                    .Include(x => x.Supplier)
                    .Include(x => x.PurchaseDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.PurchasePayments)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();

                List<PurchaseInvoiceDTO> result = new();

                foreach (var item in purchaseInvoices)
                {
                    var dto = item.ToDTO();
                    dto.PurchaseDetails = item.PurchaseDetails.Select(x => x.ToDTO()).ToList();
                    dto.PurchasePayments = item.PurchasePayments.Select(x => x.ToDTO()).ToList();
                    dto.Supplier = item.Supplier.ToDTO();
                    result.Add(dto);
                }

                return Result<IEnumerable<PurchaseInvoiceDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(PurchaseInvoiceService),
                   nameof(GetAllAsync));

                return Result<IEnumerable<PurchaseInvoiceDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<PurchaseInvoiceDTO>> GetByIdAsync(int id)
        {
            try
            {
                var purchaseInvoice = await _appDbContext.PurchaseInvoices
                    .Include(x => x.Supplier)
                    .Include(x => x.PurchaseDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.PurchasePayments)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (purchaseInvoice == null)
                {
                    return Result<PurchaseInvoiceDTO>.Failure(ResultCodes.NotFound, 404);
                }

                var result = purchaseInvoice.ToDTO();
                result.PurchaseDetails = purchaseInvoice.PurchaseDetails.Select(x => x.ToDTO()).ToList();
                result.PurchasePayments = purchaseInvoice.PurchasePayments.Select(x => x.ToDTO()).ToList();
                result.Supplier = purchaseInvoice.Supplier.ToDTO();

                return Result<PurchaseInvoiceDTO>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error in Type : {Type}, Method: {Method},",
                   nameof(PurchaseInvoiceService),
                   nameof(GetByIdAsync));

                return Result<PurchaseInvoiceDTO>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }
        }

        public async Task<Result<IEnumerable<PurchaseInvoiceSearchDTO>>> SearchAsync(string search)
        {
            var query = _appDbContext.PurchaseInvoices
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.InvoiceNo.Contains(search) ||
                    (x.Notes != null && x.Notes.Contains(search)) ||
                    x.Supplier.FullName.Contains(search));
            }

            var purchaseInvoices = await query
                .Take(20)
                .Select(x => new PurchaseInvoiceSearchDTO
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();

            return Result<IEnumerable<PurchaseInvoiceSearchDTO>>.Success(purchaseInvoices);
        }

        #endregion

        #region ========================= Update =========================
        public async Task<Result<bool>> UpdateAsync(int id, PurchaseInvoiceDTO dto)
        {

            try
            {
                var validationResult = await ValidatePurchaseInvoiceDTO(dto, id);

                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(
                        validationResult.Code,
                        validationResult.StatusCode);
                }

                //var entity = dto.ToEntity();

                var purchaseInvoice = await _appDbContext.PurchaseInvoices
                    .Include(x => x.PurchaseDetails)
                    .Include(x => x.PurchasePayments)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (purchaseInvoice == null)
                {
                    return Result<bool>.Failure(ResultCodes.NotFound, 404);
                }

                purchaseInvoice.InvoiceDate = dto.InvoiceDate;
                purchaseInvoice.SupplierId = dto.SupplierId;
                purchaseInvoice.Notes = dto.Notes;
                purchaseInvoice.UpdatedAt = DateTime.UtcNow;

                _appDbContext.PurchaseDetails.RemoveRange(purchaseInvoice.PurchaseDetails);
                _appDbContext.PurchasePayments.RemoveRange(purchaseInvoice.PurchasePayments);

                purchaseInvoice.PurchaseDetails = dto.PurchaseDetails.Select(x => x.ToEntity()).ToList();
                purchaseInvoice.PurchasePayments = dto.PurchasePayments.Select(x => x.ToEntity()).ToList();

                purchaseInvoice.CalculateTotal();
                purchaseInvoice.UpdatePaymentStatus();

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(PurchaseInvoiceService),
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
                var purchaseInvoice = await _appDbContext.PurchaseInvoices
                    .Include(x => x.PurchaseDetails)
                    .Include(x => x.PurchasePayments)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (purchaseInvoice == null)
                {
                    return Result<bool>.Failure(
                        ResultCodes.NotFound,
                        HttpStatusCodes.NotFound);
                }

                var now = DateTime.UtcNow; 
                foreach (var item in purchaseInvoice.PurchaseDetails)
                {
                    item.IsDeleted = true;
                    item.UpdatedAt = now;
                    item.DeletedAt = now;
                }

                foreach (var item in purchaseInvoice.PurchasePayments)
                {
                    item.IsDeleted = true;
                    item.UpdatedAt = now;
                    item.DeletedAt = now;
                }

                purchaseInvoice.IsDeleted = true;
                purchaseInvoice.UpdatedAt = now;
                purchaseInvoice.DeletedAt = now;

                await _appDbContext.SaveChangesAsync();
                return Result<bool>.Success(true, ResultCodes.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(PurchaseInvoiceService),
                    nameof(DeleteAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500,
                    "An unexpected error occurred.");
            }

        }
        #endregion

        #region ========================= Helpers =========================
        private async Task<Result<bool>> ValidatePurchaseInvoiceDTO(
            PurchaseInvoiceDTO DTO, 
            int? excludedId = null)
        {
            if (DTO == null)
            {
                return Result<bool>.Failure(
                    ResultCodes.InvalidData,
                    HttpStatusCodes.BadRequest);
            }

            //if (string.IsNullOrWhiteSpace(DTO.InvoiceNo))
            //{
            //    return Result<bool>.Failure(ResultCodes.InvoiceNumberRequired,
            //                                HttpStatusCodes.BadRequest);
            //}

            //var invoiceExists = await _appDbContext.PurchaseInvoices
            //    .AnyAsync(x =>
            //        x.InvoiceNo == DTO.InvoiceNo &&
            //        (excludedId == null || x.Id != excludedId));

            //if (invoiceExists)
            //{
            //    return Result<bool>.Failure(ResultCodes.InvoiceNumberExists,
            //                                HttpStatusCodes.Conflict);
            //}
                

            // Supplier
            bool supplierExists =
                await _appDbContext.Suppliers
                .AnyAsync(x => x.Id == DTO.SupplierId);

            if (!supplierExists)
            {
                return Result<bool>.Failure(
                    ResultCodes.SupplierNotFound, 
                    HttpStatusCodes.NotFound);
            }

            // Details

            if (DTO.PurchaseDetails is null || !DTO.PurchaseDetails.Any())
            {
                return Result<bool>.Failure(
                    ResultCodes.PurchaseDetailsRequired,
                    HttpStatusCodes.BadRequest);
            }

            // Duplicate products
            if (DTO.PurchaseDetails
                .GroupBy(x => x.ProductId)
                .Any(g => g.Count() > 1))
            {
                return Result<bool>.Failure(
                    ResultCodes.DuplicateProductInInvoice,
                    HttpStatusCodes.BadRequest);
            }

            // Quantity
            if (DTO.PurchaseDetails.Any(x => x.Quantity <= 0))
            {
                return Result<bool>.Failure(
                        ResultCodes.InvalidQuantity,
                        HttpStatusCodes.BadRequest);
            }

            // Unit Price
            if (DTO.PurchaseDetails.Any(x => x.UnitPrice <= 0))
            {
                return Result<bool>.Failure(
                        ResultCodes.InvalidUnitPrice,
                        HttpStatusCodes.BadRequest);
            }

            // Product existence
            var productIds = DTO.PurchaseDetails
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var existingProductIds = await _appDbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (existingProductIds.Count != productIds.Count)
            {
                return Result<bool>.Failure(
                        ResultCodes.ProductNotFound, 
                        HttpStatusCodes.NotFound);
            }

            // Payments
            if (DTO.PurchasePayments != null && DTO.PurchasePayments.Any())
            {

                if (DTO.PurchasePayments.Any(x => x.Amount <= 0))
                {
                    return Result<bool>.Failure(
                            ResultCodes.InvalidPaymentAmount,
                            HttpStatusCodes.BadRequest);
                }

                if (DTO.PurchasePayments.Any(x => x.PaymentDate == default))
                {
                    return Result<bool>.Failure(
                            ResultCodes.InvalidPaymentDate,
                            HttpStatusCodes.BadRequest);
                }

                // prevent paying more than the invoice total
                var invoiceTotal = DTO.PurchaseDetails.Sum(x => x.Quantity * x.UnitPrice);
                var totalPaid = DTO.PurchasePayments.Sum(x => x.Amount);

                if (totalPaid > invoiceTotal)
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

            var prefix = $"PUR-{now:yyyy-MM}-";

            var lastInvoiceNo = await _appDbContext.PurchaseInvoices
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

        #endregion

    }
}
