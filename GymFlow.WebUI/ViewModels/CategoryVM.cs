using System.ComponentModel.DataAnnotations;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.WebUI.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

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
            Name = nameof(SharedResource.IsActive),
            ResourceType = typeof(SharedResource)
        )]
        public bool IsActive { get; set; } = true;


        public ImageInputVM Image { get; set; } = new();

    }
}
