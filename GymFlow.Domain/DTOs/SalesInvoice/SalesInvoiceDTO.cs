using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SalesInvoice
{
    public class SalesInvoiceDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceNo),
            ResourceType = typeof(SharedResource)
        )]
        public string InvoiceNo { get; set; } = string.Empty;

        [Display(
            Name = nameof(SharedResource.Member),
            ResourceType = typeof(SharedResource)
        )]
        public int? MemberId { get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceDate),
            ResourceType = typeof(SharedResource)
        )]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Display(
            Name = nameof(SharedResource.SubTotal),
            ResourceType = typeof(SharedResource)
        )]
        public decimal SubTotal { get; set; }

        [Display(
            Name = nameof(SharedResource.Discount),
            ResourceType = typeof(SharedResource)
        )]
        public decimal Discount { get; set; }

        [Display(
            Name = nameof(SharedResource.Tax),
            ResourceType = typeof(SharedResource)
        )]
        public decimal Tax { get; set; }

        [Display(
            Name = nameof(SharedResource.NetAmount),
            ResourceType = typeof(SharedResource)
        )]
        public decimal NetAmount { get; set; }

        [Display(
            Name = nameof(SharedResource.PaidAmount),
            ResourceType = typeof(SharedResource)
        )]
        public decimal PaidAmount { get; set; }

        [Display(
            Name = nameof(SharedResource.RemainingBalance),
            ResourceType = typeof(SharedResource)
        )]
        public decimal RemainingBalance { get; set; }

        [Display(
            Name = nameof(SharedResource.Status),
            ResourceType = typeof(SharedResource)
        )]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        [Display(
            Name = nameof(SharedResource.Notes),
            ResourceType = typeof(SharedResource)
        )]
        public string? Notes { get; set; }

        // Navigation

        public MemberDTO? Member { get; set; }
        public ICollection<SalesInvoiceDetailDTO> Details { get; set; } = new List<SalesInvoiceDetailDTO>();
        public ICollection<SalesPaymentDTO> Payments { get; set; } = new List<SalesPaymentDTO>();

    }
}
