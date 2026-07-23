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
    public class PurchasePaymentConfiguration : IEntityTypeConfiguration<PurchasePayment>
    {
        public void Configure(EntityTypeBuilder<PurchasePayment> builder)
        {
            builder.ToTable("PurchasePayments");

            builder.HasKey(x => x.Id);

            //builder.HasIndex(x => x.PurchaseInvoiceId)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.PaymentMethod)
                .IsRequired(true)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Cash\n2. Bankak\n3. Fawry")
                .HasColumnType("varchar(20)");

            builder.Property(x => x.ReferenceNo)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(LoadPurchasePayments());
        }

        

private static List<PurchasePayment> LoadPurchasePayments()
        {
            return new()
    {
        new PurchasePayment
        {
            Id = 1,
            PurchaseInvoiceId = 1,
            Amount = 2000,
            PaymentMethod = PaymentMethod.Bankak,
            PaymentDate = new DateTime(2026, 1, 10),
            ReferenceNo = "TRX-10001"
        },
        new PurchasePayment
        {
            Id = 2,
            PurchaseInvoiceId = 1,
            Amount = 1500,
            PaymentMethod = PaymentMethod.Fawry,
            PaymentDate = new DateTime(2026, 1, 10),
            ReferenceNo = "TRX-10002"
        },
        new PurchasePayment
        {
            Id = 3,
            PurchaseInvoiceId = 2,
            Amount = 1000,
            PaymentMethod = PaymentMethod.Bankak,
            PaymentDate = new DateTime(2026, 1, 12),
            ReferenceNo = "TRX-10003"
        }
    };
        }
    }

}
