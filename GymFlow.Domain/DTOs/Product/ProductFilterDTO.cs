using GymFlow.Domain.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Product
{
    public class ProductFilterDTO : BaseFilterDTO
    {
        public int? CategoryId { get; set; }
        public decimal? MinPurchasePrice { get; set; }
        public decimal? MaxPurchasePrice { get; set; }
        public decimal? MinSalePrice { get; set; }
        public decimal? MaxSalePrice { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int? MinReorderLevel { get; set; }
        public int? MaxReorderLevel { get; set; }
        public bool LowStockOnly { get; set; } = false;
        public bool OutOfStockOnly { get; set; } = false;

    }
}
