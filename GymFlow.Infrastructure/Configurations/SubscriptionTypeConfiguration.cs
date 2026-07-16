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


            builder.HasData(LoadSubscriptionTypes());
        }
        
        private static List<SubscriptionType> LoadSubscriptionTypes()
        {
            return new List<SubscriptionType>
            {
                new SubscriptionType
                {
                    Id = 1,
                    NameEn = "Monthly",
                    NameAr = "شهري",
                    DaysPerWeek = 7,
                    DurationDays = 30,
                    Price = 50,
                    IsActive = true
                },
                new SubscriptionType
                {
                    Id = 2,
                    NameEn = "Quarterly",
                    NameAr = "ربع سنوي",
                    DaysPerWeek = 7,
                    DurationDays = 90,
                    Price = 130,
                    IsActive = true
                },
                new SubscriptionType
                {
                    Id = 3,
                    NameEn = "Yearly",
                    NameAr = "سنوي",
                    DaysPerWeek = 7,
                    DurationDays = 365,
                    Price = 450,
                    IsActive = true
                }
            };
        }

    }

}
