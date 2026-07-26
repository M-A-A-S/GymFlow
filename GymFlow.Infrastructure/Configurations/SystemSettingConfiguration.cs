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
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NameAr)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(SystemSettings());

        }


        private static List<SystemSetting> SystemSettings()
        {
            return new()
        {
            new SystemSetting
{
    Id = 1,

    NameEn = "GymFlow Fitness Center",
    NameAr = "جيم فلو",

    AddressEn = "Khartoum, Sudan",
    AddressAr = "الخرطوم، السودان",

    Phone = "249912345678",

    Email = "info@gymflow.com",

    Website = "www.gymflow.com",

    Facebook = "gymflow",

    Instagram = "@gymflow",

    Currency = "SDG",

    ReceiptFooterEn = "Thank you for choosing GymFlow",

    ReceiptFooterAr = "شكراً لاختياركم جيم فلو",

    TaxNumber = "",

    LogoUrl = "/images/logo.png"
}
        };
        }


    }
}
