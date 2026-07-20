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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NameAr)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(500);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(LoadCategories());
        }

        private static List<Category> LoadCategories()
        {
            return new()
        {
            new Category
            {
                Id = 1,
                NameEn = "Supplements",
                NameAr = "المكملات الغذائية"
            },
            new Category
            {
                Id = 2,
                NameEn = "Accessories",
                NameAr = "الإكسسوارات"
            }
        };
        }

    }
}
