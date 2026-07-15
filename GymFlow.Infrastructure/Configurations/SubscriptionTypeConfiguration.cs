using GymFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymFlow.Infrastructure.Configurations
{
    public class SubscriptionTypeConfiguration : IEntityTypeConfiguration<SubscriptionType>
    {
        public void Configure(EntityTypeBuilder<SubscriptionType> builder)
        {
            builder.ToTable("SubscriptionTypes");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.NameEn)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(st => st.NameAr)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(st => st.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(st => st.DaysPerWeek)
                .IsRequired();

            builder.Property(st => st.DurationDays)
                .IsRequired();

            builder.Property(st => st.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(st => st.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }

}
