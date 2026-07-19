using GymFlow.Domain.Entities;
using GymFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Helpers
{
    public class TestDbContext : DbContext, IAppDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            ApplyGlobalSoftDeleteFilter(modelBuilder);
        }

        private static void ApplyGlobalSoftDeleteFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Not(property);
                    var lambda = Expression.Lambda(condition, parameter);

                    entityType.SetQueryFilter(lambda);
                }
            }
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<MemberSubscription> MemberSubscriptions { get; set; }
        public DbSet<MemberAttendance> MemberAttendances { get; set; }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerSchedule> TrainerSchedules { get; set; }
        public DbSet<GymSchedule> GymSchedules { get; set; }

        public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
