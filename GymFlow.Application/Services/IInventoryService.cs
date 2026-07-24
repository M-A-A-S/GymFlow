using GymFlow.Domain.DTOs.Inventory;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IInventoryService
    {
        Task<Result<bool>> IncreaseStockAsync(
        IEnumerable<StockMovementDTO> items);

        Task<Result<bool>> DecreaseStockAsync(
            IEnumerable<StockMovementDTO> items);

        Task<Result<bool>> ApplyStockMovementsAsync(
            IEnumerable<StockMovementDTO> items);

    }
}
