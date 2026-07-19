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
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {

        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.ToTable("Trainers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Salary)
                .HasPrecision(18, 2);

            builder.Property(x => x.HireDate)
                .HasColumnType("date");

            builder.HasMany(x => x.TrainerSchedules)
                   .WithOne(x => x.Trainer)
                   .HasForeignKey(x => x.TrainerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadTrainers());
        }


        private static List<Trainer> LoadTrainers()
        {
            return new List<Trainer>
    {
        new Trainer
        {
            Id = 1,
            FullName = "Ahmed Mohamed",
            PhoneNumber = "01000000101",
            Salary = 5000,
            HireDate = new DateOnly(2025, 1, 10)
        },

        new Trainer
        {
            Id = 2,
            FullName = "Sara Ali",
            PhoneNumber = "01000000102",
            Salary = 5500,
            HireDate = new DateOnly(2025, 3, 15)
        },

        new Trainer
        {
            Id = 3,
            FullName = "Mohamed Hassan",
            PhoneNumber = "01000000103",
            Salary = 6000,
            HireDate = new DateOnly(2024, 11, 20)
        }
    };
        }

    }
}
