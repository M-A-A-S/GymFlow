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
                .IsRequired();

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
    }

}
