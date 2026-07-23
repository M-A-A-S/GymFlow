using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.DTOs.PurchasePayment;
using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.Domain.DTOs.PurchaseInvoice
{
    public class PurchaseInvoiceDTO
    {
        public int Id {  get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceNo),
            ResourceType = typeof(SharedResource)
        )]
        public string InvoiceNo { get; set; } = string.Empty;

        [Display(
            Name = nameof(SharedResource.Supplier),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public int SupplierId { get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceDate),
            ResourceType = typeof(SharedResource)
        )]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Display(
            Name = nameof(SharedResource.TotalAmount),
            ResourceType = typeof(SharedResource)
        )]
        public decimal TotalAmount { get; set; }

        [Display(
            Name = nameof(SharedResource.PaymentStatus),
            ResourceType = typeof(SharedResource)
        )]
        public PaymentStatus PaymentStatus { get; set; }

        [Display(
            Name = nameof(SharedResource.Notes),
            ResourceType = typeof(SharedResource)
        )]
        public string? Notes { get; set; }

        public SupplierDTO? Supplier { get; set; } = null!;
        public ICollection<PurchaseDetailDTO> PurchaseDetails { get; set; } = new List<PurchaseDetailDTO>();
        public ICollection<PurchasePaymentDTO> PurchasePayments { get; set; } = new List<PurchasePaymentDTO>();

    }
}
