using GymFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymFlow.Infrastructure.Configurations
{
    public class MemberAttendanceConfiguration : IEntityTypeConfiguration<MemberAttendance>
    {
        public void Configure(EntityTypeBuilder<MemberAttendance> builder)
        {
            builder.ToTable("MemberAttendances");

            builder.HasKey(ma => ma.Id);

            builder.Property(ma => ma.AttendanceDate)
                .IsRequired();

            builder.Property(ma => ma.CheckIn)
                .IsRequired();

            builder.Property(ma => ma.CheckOut)
                .IsRequired(false);

            builder.Property(ma => ma.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ma => ma.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships

            builder.HasOne(ma => ma.Member)
                .WithMany(m => m.MemberAttendances)
                .HasForeignKey(ma => ma.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static List<MemberAttendance> LoadMemberAttendances()
        {
            return new List<MemberAttendance>
            {
                new MemberAttendance
                {
                    Id = 1,
                    MemberId = 1,
                    AttendanceDate = new DateOnly(2026, 1, 15),
                    CheckIn = new TimeOnly(8, 0),
                    CheckOut = new TimeOnly(10, 0)
                },

                new MemberAttendance
                {
                    Id = 2,
                    MemberId = 1,
                    AttendanceDate = new DateOnly(2026, 1, 16),
                    CheckIn = new TimeOnly(7, 30),
                    CheckOut = new TimeOnly(9, 30)
                },

                new MemberAttendance
                {
                    Id = 3,
                    MemberId = 2,
                    AttendanceDate = new DateOnly(2026, 1, 15),
                    CheckIn = new TimeOnly(17, 0),
                    CheckOut = new TimeOnly(18, 30)
                }
            };
        }

    }

}
