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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
            .HasMaxLength(150)
            .IsRequired()
            .HasColumnType("nvarchar(150)");

            builder.Property(x => x.NameAr)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(500);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.PurchasePrice)
            .HasPrecision(18, 2);

            builder.Property(x => x.SalePrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.Quantity)
                .HasDefaultValue(0);

            builder.Property(x => x.ReorderLevel)
                .HasDefaultValue(0);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(LoadProducts());
        }

        private static List<Product> LoadProducts()
        {
            return new()
        {
            new Product
            {
                Id = 1,
                Code = "PRD-000001",
                NameEn = "Whey Protein",
                NameAr = "بروتين",
                PurchasePrice = 25000,
                SalePrice = 35000,
                Quantity = 30,
                ReorderLevel = 5,
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Code = "PRD-000002",
                NameEn = "Gym Gloves",
                NameAr = "قفازات",
                PurchasePrice = 7000,
                SalePrice = 10000,
                Quantity = 50,
                ReorderLevel = 10,
                CategoryId = 2
            }
        };
        }


    }
}
