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
    public class SalesInvoiceDetailConfiguration : IEntityTypeConfiguration<SalesInvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<SalesInvoiceDetail> builder)
        {
            builder.ToTable("SalesInvoiceDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemType)
                .HasConversion<int>();

            builder.Property(x => x.Description)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 2);

            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.Discount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Total)
            .HasPrecision(18, 2);

            //builder.HasOne(x => x.SalesInvoice)
            //    .WithMany(x => x.Details)
            //    .HasForeignKey(x => x.SalesInvoiceId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadSalesInvoiceDetails());

        }

        private static List<SalesInvoiceDetail> LoadSalesInvoiceDetails()
        {
            return new()
    {
        // Invoice 1
        new SalesInvoiceDetail
        {
            Id = 1,

            SalesInvoiceId = 1,

            ItemType = SaleItemType.Subscription,

            ItemId = 1,

            Description = "Monthly Gym Membership",

            Quantity = 1,

            UnitPrice = 100,

            Discount = 0,

            Total = 100
        },


        new SalesInvoiceDetail
        {
            Id = 2,

            SalesInvoiceId = 1,

            ItemType = SaleItemType.Product,

            ItemId = 1,

            Description = "Protein Powder",

            Quantity = 1,

            UnitPrice = 50,

            Discount = 0,

            Total = 50
        },


        // Invoice 2
        new SalesInvoiceDetail
        {
            Id = 3,

            SalesInvoiceId = 2,

            ItemType = SaleItemType.Subscription,

            ItemId = 2,

            Description = "Gold Membership",

            Quantity = 1,

            UnitPrice = 250,

            Discount = 20,

            Total = 230
        },


        // Invoice 3

        new SalesInvoiceDetail
        {
            Id = 4,

            SalesInvoiceId = 3,

            ItemType = SaleItemType.Product,

            ItemId = 2,

            Description = "Gym Gloves",

            Quantity = 1,

            UnitPrice = 80,

            Discount = 0,

            Total = 80
        }
    };
        }


    }
}
