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
	public class ProductRateInfoConfiguration : IEntityTypeConfiguration<ProductRateInfo>
	{
		public void Configure(EntityTypeBuilder<ProductRateInfo> builder)
		{
			builder.Property(e => e.CostPrice)
				.HasColumnType("float");

			builder.Property(e => e.SellingPrice)
				.HasColumnType("float");

			builder.Property(e => e.Quantity)
				.HasColumnType("float");

			builder.Property(e => e.SoldQuantity)
				.HasColumnType("float");

			builder.Property(e => e.RemainingQuantity)
				.HasColumnType("float");

			builder.Property(e => e.BatchNo)
				.HasMaxLength(100)
				.IsUnicode(true);

			builder.Property(e => e.PurchasedDate)
				.HasColumnType("datetime");

			builder.Property(e => e.Expirydate)
				.HasColumnType("datetime");

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

			builder.HasMany(e => e.ProductInvoiceDetailInfos)
			.WithOne(pt => pt.ProductRateInfo)
			.HasForeignKey(e => e.ProductRateInfoId);

			builder.HasMany(e => e.StockInfos)
			.WithOne(pt => pt.ProductRateInfo)
			.HasForeignKey(e => e.ProductRateInfoId);

			builder.HasMany(e => e.TransactionInfos)
			.WithOne(pt => pt.ProductRateInfo)
			.HasForeignKey(e => e.ProductRateInfoId);

			builder.HasOne(e => e.CategoryInfo)
			.WithMany(pt => pt.ProductRateInfos)
			.HasForeignKey(e => e.CategoryInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProductInfo)
			.WithMany(pt => pt.ProductRateInfos)
			.HasForeignKey(e => e.ProductInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.StoreInfo)
			.WithMany(pt => pt.ProductRateInfos)
			.HasForeignKey(e => e.StoreInfoId)
			.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.SupplierInfo)
			.WithMany(pt => pt.ProductRateInfos)
			.HasForeignKey(e => e.SupplierInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.RackInfo)
			.WithMany(pt => pt.ProductRateInfos)
			.HasForeignKey(e => e.RackInfoId)
            .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
