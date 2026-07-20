using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Constants
{
    public static partial class ResultCodes
    {

        public const string InvalidTrainer = "InvalidTrainer";
        public const string TrainerNotFound = "TrainerNotFound";

        public const string InvalidScheduleTime = "InvalidScheduleTime";
        public const string TrainerScheduleOverlap = "TrainerScheduleOverlap";

    }
}
