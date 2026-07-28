using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberSubscription
{
    public class CurrentSubscriptionDTO
    {
        public int Id { get; set; }
        public string NameEn {  get; set; }
        public string NameAr {  get; set; }
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate {  get; set; }
    }
}
