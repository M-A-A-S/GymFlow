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
    public class SalesInvoiceSearchDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceNo),
            ResourceType = typeof(SharedResource)
        )]
        public string InvoiceNo { get; set; } = string.Empty;

        [Display(
            Name = nameof(SharedResource.InvoiceDate),
            ResourceType = typeof(SharedResource)
        )]
        public DateTime InvoiceDate { get; set; }

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
        public InvoiceStatus Status { get; set; }
    }
}
