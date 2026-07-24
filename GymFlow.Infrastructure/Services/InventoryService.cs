using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
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
    public class InventoryService : IInventoryService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<InventoryService> _logger;
        #endregion

        #region ========================= Constructors =========================
        public InventoryService(IAppDbContext appDbContext,
            ILogger<InventoryService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Increase Stock =========================
        public async Task<Result<bool>> IncreaseStockAsync(IEnumerable<StockMovementDTO> items)
        {

            return await ApplyStockMovementsAsync(
                items.Select(x => new StockMovementDTO
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    MovementType = StockMovementType.Increase
                }));

        }

        #endregion

        #region ========================= Decrease Stock =========================
        public async Task<Result<bool>> DecreaseStockAsync(IEnumerable<StockMovementDTO> items)
        {

            return await ApplyStockMovementsAsync(
                items.Select(x => new StockMovementDTO
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    MovementType = StockMovementType.Decrease
                }));

        }

        #endregion

        #region ========================= Apply Stock Movements =========================
        public async Task<Result<bool>> ApplyStockMovementsAsync(IEnumerable<StockMovementDTO> items)
        {
            try
            {
                if (items == null || !items.Any())
                {
                    return Result<bool>.Success(true);
                }

                if (items.Any(x => x.Quantity <= 0))
                {
                    return Result<bool>.Failure(
                        ResultCodes.InvalidStockQuantity,
                        HttpStatusCodes.BadRequest);
                }

                var productIds = items
                    .Select(x => x.ProductId)
                    .Distinct()
                    .ToList();

                var products =
                    await _appDbContext.Products
                    .Where(x => productIds.Contains(x.Id))
                    .ToDictionaryAsync(x => x.Id);

                if (products.Count != productIds.Count)
                {
                    return Result<bool>.Failure(
                            ResultCodes.ProductNotFound,
                            HttpStatusCodes.BadRequest);
                }

                var groupedItems = items
                .GroupBy(x  => new
                {
                    x.ProductId,
                    x.MovementType,
                })
                .Select(x => new
                {
                    x.Key.ProductId,
                    x.Key.MovementType,
                    Quantity = x.Sum(y => y.Quantity)
                })
                .ToList();

                var now = DateTime.UtcNow;

                foreach (var item in groupedItems)
                {
                    //var product = products.GetValueOrDefault(item.ProductId);
                    var product = products[item.ProductId];

                    switch (item.MovementType)
                    {
                        case StockMovementType.Increase:
                            product.Quantity += (int)item.Quantity;
                            break;

                        case StockMovementType.Decrease:
                            if (product.Quantity < item.Quantity)
                            {
                                return Result<bool>.Failure(
                                    ResultCodes.InsufficientStock,
                                    HttpStatusCodes.BadRequest);
                            }

                            product.Quantity -= (int)item.Quantity;
                            break;

                        default:

                            return Result<bool>.Failure(
                                ResultCodes.InvalidStockMovementType,
                                HttpStatusCodes.BadRequest);

                    }

                    product.UpdatedAt = now;
                }

                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(InventoryService),
                    nameof(ApplyStockMovementsAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    HttpStatusCodes.InternalServerError,
                    "An unexpected error occurred.");
            }
        }
        #endregion


    }
}
