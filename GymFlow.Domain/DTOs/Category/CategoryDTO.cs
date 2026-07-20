using GymFlow.Domain.DTOs.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameEn),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameEn { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameAr),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameAr { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.DescriptionEn),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string? DescriptionEn { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.DescriptionAr),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string? DescriptionAr { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Image),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string? ImageUrl { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.IsActive),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public bool IsActive { get; set; } = true;

        

        public FileUploadRequest? Image {  get; set; }

    }
}
