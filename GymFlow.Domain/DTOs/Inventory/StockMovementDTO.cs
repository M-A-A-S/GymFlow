using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Inventory
{
    public class StockMovementDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public StockMovementType MovementType { get; set; }
    }
}
