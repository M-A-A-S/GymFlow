using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class SalesPayment : BaseEntity
    {
        public int SalesInvoiceId { get; set; }

         //Cash account, bank account, etc.
        //public int AccountId { get; set; }

        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? ReferenceNo { get; set; }
        public string? Notes { get; set; }

        // Accounting control

        public bool IsVoided { get; set; } = false;
        public DateTime? VoidDate { get; set; }
        public string? VoidReason { get; set; }
        public int? VoidedBy { get; set; }

        // Navigation

        public SalesInvoice SalesInvoice { get; set; } = null!;
        //public Account Account { get; set; } = null!;

    }
}
