using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Trainer
{
    public class TrainerSearchDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.FullName),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string FullName { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.PhoneNumber),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string PhoneNumber { get; set; }

    }
}
