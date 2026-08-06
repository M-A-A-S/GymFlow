using GymFlow.Domain.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Trainer
{
    public class TrainerFilterDTO : BaseFilterDTO
    {
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }

        public DateOnly? HireDateFrom { get; set; }
        public DateOnly? HireDateTo { get; set; }
    }
}
