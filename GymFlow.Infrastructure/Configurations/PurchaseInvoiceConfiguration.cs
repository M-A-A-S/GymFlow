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
    public class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
        {
            builder.ToTable("PurchaseInvoices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNo)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.InvoiceNo)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            //builder.HasIndex(x => x.SupplierId)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.PaymentStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Unpaid\n2. Partial\n3. Paid")
                .HasColumnType("varchar(20)");

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Supplier)
                .WithMany(x => x.PurchaseInvoices)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PurchaseDetails)
                .WithOne(x => x.PurchaseInvoice)
                .HasForeignKey(x => x.PurchaseInvoiceId);
            //.OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.PurchasePayments)
                .WithOne(x => x.PurchaseInvoice)
                .HasForeignKey(x => x.PurchaseInvoiceId);
            //.OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadPurchaseInvoices());
        }

        private static List<PurchaseInvoice> LoadPurchaseInvoices()
        {
            return new()
    {
        new PurchaseInvoice
        {
            Id = 1,
            InvoiceNo = "PI-000001",
            SupplierId = 1,
            InvoiceDate = new DateTime(2026, 1, 10),
            TotalAmount = 3500m,
            PaymentStatus = PaymentStatus.Paid
        },
        new PurchaseInvoice
        {
            Id = 2,
            InvoiceNo = "PI-000002",
            SupplierId = 2,
            InvoiceDate = new DateTime(2026, 1, 12),
            TotalAmount = 1800m,
            PaymentStatus = PaymentStatus.Partial
        },
        new PurchaseInvoice
        {
            Id = 3,
            InvoiceNo = "PI-000003",
            SupplierId = 1,
            InvoiceDate = new DateTime(2026, 1, 15),
            TotalAmount = 950m,
            PaymentStatus = PaymentStatus.Unpaid
        }
    };
        }
    }

}
