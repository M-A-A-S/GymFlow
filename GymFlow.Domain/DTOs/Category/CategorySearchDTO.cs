using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Category
{
    public class CategorySearchDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameEn),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameEn { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameAr),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameAr { get; set; }

    }
}
