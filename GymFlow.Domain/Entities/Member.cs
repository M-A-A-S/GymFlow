using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class Member : BaseEntity
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateOnly? RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public MemberStatus Status { get; set; } = MemberStatus.Active;

        public ICollection<MemberSubscription> MemberSubscriptions { get; set; }
        public ICollection<MemberAttendance> MemberAttendances { get; set; }

    }
}
