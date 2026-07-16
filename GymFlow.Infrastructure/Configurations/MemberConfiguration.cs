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
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(m => m.Email)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("varchar(15)");

            builder.Property(x => x.Address)
                .IsRequired(false)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(x => x.BirthDate)
                .IsRequired(false);

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Active\n2. Inactive\n3. Suspended\n4. Unsuspended")
                .HasColumnType("varchar(20)");



            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(LoadMembers());

        }

        private static List<Member> LoadMembers()
        {
            return new List<Member>
            {
                new Member
                {
                    Id = 1,
                    FullName = "Ahmed Mohamed",
                    Gender = Gender.Male,
                    BirthDate = new DateOnly(1995, 5, 15),
                    PhoneNumber = "01000000001",
                    Email = "ahmed@gmail.com",
                    Address = "Khartoum",
                    RegisterDate = new DateOnly(2026, 1, 1),
                    Status = MemberStatus.Active
                },

                new Member
                {
                    Id = 2,
                    FullName = "Sara Ali",
                    Gender = Gender.Female,
                    BirthDate = new DateOnly(1998, 8, 20),
                    PhoneNumber = "01000000002",
                    Email = "sara@gmail.com",
                    Address = "Omdurman",
                    RegisterDate = new DateOnly(2026, 1, 5),
                    Status = MemberStatus.Active
                },

                new Member
                {
                    Id = 3,
                    FullName = "Mohamed Hassan",
                    Gender = Gender.Male,
                    BirthDate = new DateOnly(1990, 3, 10),
                    PhoneNumber = "01000000003",
                    Email = "mohamed@gmail.com",
                    Address = "Bahri",
                    RegisterDate = new DateOnly(2026, 1, 10),
                    Status = MemberStatus.Suspended
                }

            };
        }

    }

}
