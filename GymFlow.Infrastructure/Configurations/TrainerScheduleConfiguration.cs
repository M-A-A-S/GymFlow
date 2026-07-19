using GymFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Configurations
{
    public class TrainerScheduleConfiguration : IEntityTypeConfiguration<TrainerSchedule>
    {
        public void Configure(EntityTypeBuilder<TrainerSchedule> builder)
        {
            builder.ToTable("TrainerSchedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Day)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.StartTime)
                   .IsRequired();

            builder.Property(x => x.EndTime)
                   .IsRequired();

            // Prevent duplicate schedules for a trainer
            builder.HasIndex(x => new
            {
                x.TrainerId,
                x.Day,
                x.StartTime
            }).IsUnique()
            .HasFilter("[IsDeleted] = 0");

            builder.HasData(LoadTrainerSchedules());

        }

        private static List<TrainerSchedule> LoadTrainerSchedules()
        {
            return new List<TrainerSchedule>
    {
        new TrainerSchedule
        {
            Id = 1,
            TrainerId = 1,
            Day = DayOfWeek.Sunday,
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(12, 0, 0)
        },

        new TrainerSchedule
        {
            Id = 2,
            TrainerId = 1,
            Day = DayOfWeek.Tuesday,
            StartTime = new TimeSpan(2, 0, 0),
            EndTime = new TimeSpan(6, 0, 0)
        },

        new TrainerSchedule
        {
            Id = 3,
            TrainerId = 2,
            Day = DayOfWeek.Monday,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(1, 0, 0)
        },

        new TrainerSchedule
        {
            Id = 4,
            TrainerId = 2,
            Day = DayOfWeek.Wednesday,
            StartTime = new TimeSpan(3, 0, 0),
            EndTime = new TimeSpan(7, 0, 0)
        },

        new TrainerSchedule
        {
            Id = 5,
            TrainerId = 3,
            Day = DayOfWeek.Thursday,
            StartTime = new TimeSpan(4, 0, 0),
            EndTime = new TimeSpan(8, 0, 0)
        }
    };
        }
    }
}
