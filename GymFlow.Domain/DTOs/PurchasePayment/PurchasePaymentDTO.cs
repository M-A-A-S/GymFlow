using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.PurchasePayment
{
    public class PurchasePaymentDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.PurchaseInvoice),
            ResourceType = typeof(SharedResource)
        )]
        public int PurchaseInvoiceId { get; set; }

        [Display(
            Name = nameof(SharedResource.Amount),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal Amount { get; set; }

        [Display(
            Name = nameof(SharedResource.PaymentMethod),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(
            Name = nameof(SharedResource.PaymentDate),
            ResourceType = typeof(SharedResource)
        )]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Display(
            Name = nameof(SharedResource.ReferenceNo),
            ResourceType = typeof(SharedResource)
        )]
        public string? ReferenceNo { get; set; }

        [Display(
            Name = nameof(SharedResource.Notes),
            ResourceType = typeof(SharedResource)
        )]
        public string? Notes { get; set; }

        public PurchaseInvoiceDTO? PurchaseInvoice { get; set; } = null!;

    }
}
