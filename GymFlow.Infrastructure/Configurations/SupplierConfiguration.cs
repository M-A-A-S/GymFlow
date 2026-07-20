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
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnType("nvarchar(150)");

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Address)
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnType("nvarchar(250)");

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(LoadSuppliers());
        }

        private static List<Supplier> LoadSuppliers()
        {
            return new()
        {
            new Supplier
            {
                Id = 1,
                FullName = "Fitness Nutrition Co.",
                PhoneNumber = "01000001001",
                Address = "Khartoum"
            },
            new Supplier
            {
                Id = 2,
                FullName = "Elite Sports Supplies",
                PhoneNumber = "01000001002",
                Address = "Omdurman"
            }
        };
        }

    }
}
