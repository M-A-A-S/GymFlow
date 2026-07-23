using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class PurchasePayment : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }
        //public int? AccountId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod {  get; set; }
        public DateTime PaymentDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Notes { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; } = null!;

    }
}
