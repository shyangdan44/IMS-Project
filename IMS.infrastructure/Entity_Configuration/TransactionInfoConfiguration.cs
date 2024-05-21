using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.infrastructure.Entity_Configuration
{
	public class TransactionInfoConfiguration : IEntityTypeConfiguration<TransactionInfo>
	{
		public void Configure(EntityTypeBuilder<TransactionInfo> builder)
		{
			builder.Property(e => e.TransactionType)
				.HasMaxLength(200)
				.IsUnicode(true);

			builder.Property(e => e.Rate)
				.HasColumnType("float");

			builder.Property(e => e.Quantity)
				.HasColumnType("float");

			builder.Property(e => e.Amount)
				.HasColumnType("float");

			builder.Property(e => e.IsActive)
			.HasDefaultValue(true);

			builder.Property(e => e.CreatedDate)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");
			builder.Property(e => e.CreatedBy)
				.IsRequired()
				.IsUnicode(true);
			builder.Property(e => e.ModifiedDate)
				.HasColumnType("datetime");

			builder.Property(e => e.ModifiedBy)
				.IsUnicode(true);

			builder.HasOne(e => e.CategoryInfo)
			.WithMany(pt => pt.TransactionInfos)
			.HasForeignKey(e => e.CategoryInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProductInfo)
			.WithMany(pt => pt.TransactionInfos)
			.HasForeignKey(e => e.ProductInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProductRateInfo)
			.WithMany(pt => pt.TransactionInfos)
			.HasForeignKey(e => e.ProductRateInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UnitInfo)
			.WithMany(pt => pt.TransactionInfos)
			.HasForeignKey(e => e.UnitInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.StoreInfo)
			.WithMany(pt => pt.TransactionInfos)
			.HasForeignKey(e => e.StoreInfoId)
            .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
