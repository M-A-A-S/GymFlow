using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Supplier
{
    public class SupplierDTO
    {

        public int Id { get; set; } 

        [Display(
            Name = nameof(Resources.Shared.SharedResource.FullName),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string FullName { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.PhoneNumber),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string PhoneNumber { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Address),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string Address { get; set; }

    }
}
