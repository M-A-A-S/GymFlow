using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class SalesInvoice : BaseEntity
    {
        public string InvoiceNo { get; set; } = string.Empty;
        public int? MemberId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
        public string? Notes { get; set; }

        // Navigation

        public Member? Member { get; set; }
        public ICollection<SalesInvoiceDetail> Details { get; set; } = new List<SalesInvoiceDetail>();
        public ICollection<SalesPayment> Payments { get; set; } = new List<SalesPayment>();
    }
}
