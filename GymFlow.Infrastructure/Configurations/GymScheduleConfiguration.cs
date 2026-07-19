using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Configurations
{
    public class GymScheduleConfiguration : IEntityTypeConfiguration<GymSchedule>
    {
        public void Configure(EntityTypeBuilder<GymSchedule> builder)
        {
            builder.ToTable("GymSchedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Day)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Gender)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.Day,
                x.Gender,
                x.StartTime,
            })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

            builder.HasData(LoadGymSchedules());

        }

        private static List<GymSchedule> LoadGymSchedules()
        {
            return new List<GymSchedule>
    {
        // Sunday
        new GymSchedule
        {
            Id = 1,
            Day = DayOfWeek.Sunday,
            Gender = Gender.Male,
            StartTime = new TimeSpan(16, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        },

        new GymSchedule
        {
            Id = 2,
            Day = DayOfWeek.Sunday,
            Gender = Gender.Female,
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(15, 0, 0)
        },

        // Monday
        new GymSchedule
        {
            Id = 3,
            Day = DayOfWeek.Monday,
            Gender = Gender.Male,
            StartTime = new TimeSpan(16, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        },

        new GymSchedule
        {
            Id = 4,
            Day = DayOfWeek.Monday,
            Gender = Gender.Female,
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(15, 0, 0)
        },

        // Tuesday
        new GymSchedule
        {
            Id = 5,
            Day = DayOfWeek.Tuesday,
            Gender = Gender.Male,
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        },

        new GymSchedule
        {
            Id = 6,
            Day = DayOfWeek.Tuesday,
            Gender = Gender.Female,
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(17, 0, 0)
        }
    };
        }
    }
}
