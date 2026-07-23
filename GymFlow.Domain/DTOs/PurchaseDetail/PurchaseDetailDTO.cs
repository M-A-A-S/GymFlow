using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.PurchaseDetail
{
    public class PurchaseDetailDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.PurchaseInvoice),
            ResourceType = typeof(SharedResource)
        )]
        public int PurchaseInvoiceId { get; set; }

        [Display(
            Name = nameof(SharedResource.Product),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public int ProductId { get; set; }

        [Display(
            Name = nameof(SharedResource.Quantity),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal Quantity { get; set; }

        [Display(
            Name = nameof(SharedResource.UnitPrice),
            ResourceType = typeof(SharedResource)
        )]
        public decimal UnitPrice { get; set; }

        [Display(
            Name = nameof(SharedResource.Total),
            ResourceType = typeof(SharedResource)
        )]
        public decimal Total { get; set; }

        public PurchaseInvoiceDTO? PurchaseInvoice { get; set; } = null!;
        public ProductDTO? Product { get; set; } = null!;

    }
}
