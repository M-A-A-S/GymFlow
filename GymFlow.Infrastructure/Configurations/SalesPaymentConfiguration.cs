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
    public class SalesPaymentConfiguration : IEntityTypeConfiguration<SalesPayment>
    {
        public void Configure(EntityTypeBuilder<SalesPayment> builder)
        {
            builder.ToTable("SalesPayments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.ReferenceNo)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.PaymentMethod)
                .IsRequired(true)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Cash\n2. Bankak\n3. Fawry")
                .HasColumnType("varchar(20)");

            //builder.HasOne(x => x.SalesInvoice)
            //.WithMany(x => x.Payments)
            //.HasForeignKey(x => x.SalesInvoiceId)
            //.OnDelete(DeleteBehavior.Restrict);


            //builder.HasOne(x => x.Account)
            //    .WithMany()
            //    .HasForeignKey(x => x.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(LoadSalesPayments());

        }

        private static List<SalesPayment> LoadSalesPayments()
        {
            return new()
    {
        // Invoice 1 fully paid

        new SalesPayment
        {
            Id = 1,

            SalesInvoiceId = 1,

            //AccountId = 1, // Cash Box

            Amount = 150,

            PaymentMethod = PaymentMethod.Cash,

            PaymentDate = new DateTime(2026,1,10),

            ReferenceNo = "REC-0001",

            Notes = "Full payment"
        },


        // Invoice 2 partial payment

        new SalesPayment
        {
            Id = 2,

            SalesInvoiceId = 2,

            //AccountId = 1,

            Amount = 100,

            PaymentMethod = PaymentMethod.Cash,

            PaymentDate = new DateTime(2026,1,15),

            ReferenceNo = "REC-0002",

            Notes = "First payment"
        },


        // Invoice 3 unpaid
        // No payment record
    };
        }
    }
}
