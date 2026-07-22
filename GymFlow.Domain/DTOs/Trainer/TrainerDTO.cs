using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Trainer
{
    public class TrainerDTO
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
        [RegularExpression(
            @"^(?:\+249|0)(?:[1-9][0-9])[0-9]{7}$",
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.InvalidPhone),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string PhoneNumber { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Salary),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public decimal Salary { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.HireDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly HireDate { get; set; }

    }
}
