using GymFlow.Domain.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Category
{
    public class CategoryFilterDTO : BaseFilterDTO
    {
        public bool? IsActive { get; set; } 
    }
}
