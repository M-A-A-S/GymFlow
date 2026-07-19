using GymFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Data
{
    public interface IAppDbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<MemberSubscription> MemberSubscriptions { get; set; }
        public DbSet<MemberAttendance> MemberAttendances { get; set; }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerSchedule> TrainerSchedules { get; set; }
        public DbSet<GymSchedule> GymSchedules { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
