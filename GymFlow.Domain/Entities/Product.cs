using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }

        public Category? Category { get; set; }

    }
}
