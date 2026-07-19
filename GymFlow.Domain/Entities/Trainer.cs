using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class Trainer : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        public DateOnly HireDate { get; set; }

        public ICollection<TrainerSchedule> TrainerSchedules { get; set; } 
            = new List<TrainerSchedule>();
    }
}
