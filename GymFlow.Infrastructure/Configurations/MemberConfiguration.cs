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
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(m => m.Email)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnType("varchar(15)");

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("1. Active\n2. Inactive\n3. Suspended\n4. Unsuspended")
                .HasColumnType("varchar(20)");



            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }

}
