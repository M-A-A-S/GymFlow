using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.Domain.DTOs.SalesPayment
{
    public class SalesPaymentDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.SalesInvoice),
            ResourceType = typeof(SharedResource)
        )]
        public int SalesInvoiceId { get; set; }

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

        // Accounting control


        [Display(
            Name = nameof(SharedResource.IsVoided),
            ResourceType = typeof(SharedResource)
        )]
        public bool IsVoided { get; set; } = false;

        [Display(
            Name = nameof(SharedResource.VoidDate),
            ResourceType = typeof(SharedResource)
        )]
        public DateTime? VoidDate { get; set; }

        [Display(
            Name = nameof(SharedResource.VoidReason),
            ResourceType = typeof(SharedResource)
        )]
        public string? VoidReason { get; set; }

        [Display(
            Name = nameof(SharedResource.VoidedBy),
            ResourceType = typeof(SharedResource)
        )]
        public int? VoidedBy { get; set; }

        // Navigation
        public SalesInvoiceDTO? SalesInvoice { get; set; } = null!;

    }
}
