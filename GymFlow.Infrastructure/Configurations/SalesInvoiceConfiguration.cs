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
    public class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
    {
        public void Configure(EntityTypeBuilder<SalesInvoice> builder)
        {
            builder.ToTable("SalesInvoices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNo)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(x => x.InvoiceNo)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(x => x.Discount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Tax)
                .HasPrecision(18, 2);

            builder.Property(x => x.NetAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.PaidAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.RemainingBalance)
                .HasPrecision(18, 2);

            builder.Property(x => x.Notes)
            .HasMaxLength(500);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Unpaid\n2. Partial\n3. Paid\n4. Cancelled")
                .HasColumnType("varchar(20)"); ;

            builder.HasOne(x => x.Member)
                .WithMany(m => m.SalesInvoices)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Details)
                .WithOne(d => d.SalesInvoice)
                .HasForeignKey(d => d.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Payments)
                .WithOne(p => p.SalesInvoice)
                .HasForeignKey(p => p.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(LoadSalesInvoices());

        }

        private static List<SalesInvoice> LoadSalesInvoices()
        {
            return new()
    {
        new SalesInvoice
        {
            Id = 1,

            InvoiceNo = "INV-0001",

            MemberId = 1,

            InvoiceDate = new DateTime(2026, 1, 10),

            SubTotal = 150,

            Discount = 0,

            Tax = 0,

            NetAmount = 150,

            PaidAmount = 150,

            RemainingBalance = 0,

            Status = InvoiceStatus.Paid,

            Notes = "Monthly membership + product"
        },


        new SalesInvoice
        {
            Id = 2,

            InvoiceNo = "INV-0002",

            MemberId = 2,

            InvoiceDate = new DateTime(2026, 1, 15),

            SubTotal = 250,

            Discount = 20,

            Tax = 0,

            NetAmount = 230,

            PaidAmount = 100,

            RemainingBalance = 130,

            Status = InvoiceStatus.Partial,

            Notes = "Gold subscription"
        },


        new SalesInvoice
        {
            Id = 3,

            InvoiceNo = "INV-0003",

            MemberId = null,

            InvoiceDate = new DateTime(2026, 1, 20),

            SubTotal = 80,

            Discount = 0,

            Tax = 0,

            NetAmount = 80,

            PaidAmount = 0,

            RemainingBalance = 80,

            Status = InvoiceStatus.Unpaid,

            Notes = "Walk-in customer"
        }
    };
        }


    }
}
