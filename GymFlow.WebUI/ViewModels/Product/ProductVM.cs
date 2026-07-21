using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.File;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.WebUI.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(SharedResource.Code),
            ResourceType = typeof(SharedResource)
        )]
        public string Code { get; set; }

        [Display(
            Name = nameof(SharedResource.NameEn),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public string NameEn { get; set; }

        [Display(
            Name = nameof(SharedResource.NameAr),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public string NameAr { get; set; }

        [Display(
            Name = nameof(SharedResource.DescriptionEn),
            ResourceType = typeof(SharedResource)
        )]
        public string? DescriptionEn { get; set; }

        [Display(
            Name = nameof(SharedResource.DescriptionAr),
            ResourceType = typeof(SharedResource)
        )]
        public string? DescriptionAr { get; set; }

        [Display(
            Name = nameof(SharedResource.Category),
            ResourceType = typeof(SharedResource)
        )]
        public int? CategoryId { get; set; }

        [Display(
            Name = nameof(SharedResource.PurchasePrice),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal PurchasePrice { get; set; }

        [Display(
            Name = nameof(SharedResource.SalePrice),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public decimal SalePrice { get; set; }

        [Display(
            Name = nameof(SharedResource.Quantity),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public int Quantity { get; set; } = 0;

        [Display(
            Name = nameof(SharedResource.ReorderLevel),
            ResourceType = typeof(SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(SharedResource.Required),
            ErrorMessageResourceType = typeof(SharedResource)
        )]
        public int ReorderLevel { get; set; }

        public CategoryDTO? Category { get; set; }

        public ImageInputVM Image { get; set; } = new();

    }
}
