using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.PurchaseInvoice
{
    public class PurchaseInvoiceSearchDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.InvoiceNo),
            ResourceType = typeof(SharedResource)
        )]
        public string InvoiceNo { get; set; } = string.Empty;

        [Display(
            Name = nameof(SharedResource.TotalAmount),
            ResourceType = typeof(SharedResource)
        )]
        public decimal TotalAmount { get; set; }

    }
}
