using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class SalesInvoiceDetail : BaseEntity
    {
        public int SalesInvoiceId { get; set; }
        public SaleItemType ItemType { get; set; }

        /// <summary>
        /// Id of the Product, Subscription, Service, etc.
        /// </summary>
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        // Navigation
        public SalesInvoice SalesInvoice { get; set; } = null!;

    }
}
