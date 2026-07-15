using GymFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GymFlow.Infrastructure.Configurations
{
    public class MemberSubscriptionConfiguration : IEntityTypeConfiguration<MemberSubscription>
    {
        public void Configure(EntityTypeBuilder<MemberSubscription> builder)
        {
            builder.ToTable("MemberSubscriptions");

            builder.HasKey(ms => ms.Id);

            builder.Property(ms => ms.StartDate)
                .IsRequired();

            builder.Property(ms => ms.EndDate)
                .IsRequired();

            builder.Property(ms => ms.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ms => ms.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("Active = 1\nInactive = 2\nExpired = 3\nCancelled = 4\nSuspended = 5\nUnsuspended = 6")
                .HasColumnType("varchar(20)");

            builder.Property(ms => ms.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ms => ms.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships

            builder.HasOne(ms => ms.Member)
                .WithMany(m => m.MemberSubscriptions)
                .HasForeignKey(ms => ms.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ms => ms.SubscriptionType)
                .WithMany(st => st.MemberSubscriptions)
                .HasForeignKey(ms => ms.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
