using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class PurchaseDetail : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
        public Product Product { get; set; } = null!;

    }
}
