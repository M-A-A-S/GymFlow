using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Enums;
using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SalesInvoiceDetail
{
    public class SalesInvoiceDetailDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.SalesInvoice),
            ResourceType = typeof(SharedResource)
        )]
        public int SalesInvoiceId { get; set; }

        [Display(
            Name = nameof(SharedResource.ItemType),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public SaleItemType ItemType { get; set; }

        [Display(
            Name = nameof(SharedResource.Item),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        /// <summary>
        /// Id of the Product, Subscription, Service, etc.
        /// </summary>
        public int ItemId { get; set; }

        [Display(
            Name = nameof(SharedResource.Description),
            ResourceType = typeof(SharedResource)
        )]
        public string? Description { get; set; }

        [Display(
            Name = nameof(SharedResource.Quantity),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal Quantity { get; set; } = 1;

        [Display(
            Name = nameof(SharedResource.UnitPrice),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal UnitPrice { get; set; }

        [Display(
            Name = nameof(SharedResource.Discount),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal Discount { get; set; }

        [Display(
            Name = nameof(SharedResource.Total),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal Total { get; set; }

        // Navigation
        public SalesInvoiceDTO? SalesInvoice { get; set; } = null!;


        [NotMapped]
        public ProductDTO? Product { get; set; }
        [NotMapped]
        public SubscriptionTypeDTO? SubscriptionType { get; set; }

    }
}
