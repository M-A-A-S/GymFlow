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
    public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.ToTable("PurchaseDetails");

            builder.HasKey(x => x.Id);

            //builder.HasIndex(x => x.PurchaseInvoiceId)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 2);

            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.Total)
                .HasPrecision(18, 2);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Product)
                .WithMany(x => x.PurchaseDetails)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(LoadPurchaseDetails());
        }

        

private static List<PurchaseDetail> LoadPurchaseDetails()
        {
            return new()
    {
        new PurchaseDetail
        {
            Id = 1,
            PurchaseInvoiceId = 1,
            ProductId = 1,
            Quantity = 10,
            UnitPrice = 200,
            Total = 2000
        },
        new PurchaseDetail
        {
            Id = 2,
            PurchaseInvoiceId = 1,
            ProductId = 2,
            Quantity = 5,
            UnitPrice = 300,
            Total = 1500
        },
        new PurchaseDetail
        {
            Id = 3,
            PurchaseInvoiceId = 2,
            ProductId = 3,
            Quantity = 6,
            UnitPrice = 300,
            Total = 1800
        },
        new PurchaseDetail
        {
            Id = 4,
            PurchaseInvoiceId = 3,
            ProductId = 4,
            Quantity = 5,
            UnitPrice = 190,
            Total = 950
        }
    };
        }
    }

}
