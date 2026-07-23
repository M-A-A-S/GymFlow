using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class PurchaseInvoice : BaseEntity
    {
        public string InvoiceNo { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }

        public Supplier Supplier { get; set; } = null!;
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
        public ICollection<PurchasePayment> PurchasePayments { get; set; } = new List<PurchasePayment>();


        public void CalculateTotal()
        {
            TotalAmount = PurchaseDetails.Sum(x => x.Total);
        }

        public void UpdatePaymentStatus()
        {
            var paid = PurchasePayments.Sum(x => x.Amount);

            PaymentStatus = paid switch
            {
                <= 0 => PaymentStatus.Unpaid,
                _ when paid < TotalAmount => PaymentStatus.Partial,
                _ => PaymentStatus.Paid
            };
        }

        public void AddPayment(PurchasePayment payment)
        {
            PurchasePayments.Add(payment);
            UpdatePaymentStatus();
        }

    }
}
