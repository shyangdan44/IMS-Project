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
	public class SupplierInfoConfiguration : IEntityTypeConfiguration<SupplierInfo>
	{
		public void Configure(EntityTypeBuilder<SupplierInfo> builder)
		{
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

            builder.Property(e => e.SupplierName)
				.HasMaxLength(200)
				.IsUnicode(true);

			builder.Property(e => e.ContactPerson)
				.HasMaxLength(200)
				.IsUnicode(true);

			builder.Property(e => e.PhoneNumber)
				.HasMaxLength(200)
				.IsUnicode(true);

			builder.Property(e => e.Address)
				.HasMaxLength(200)
				.IsUnicode(true);

			builder.Property(e => e.Email)
				.HasMaxLength(200)
				.IsUnicode(true);

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

			builder.HasMany(e => e.ProductRateInfos)
			.WithOne(pt => pt.SupplierInfo)
			.HasForeignKey(e => e.SupplierInfoId);

			builder.HasOne(e => e.StoreInfo)
			.WithMany(pt => pt.SupplierInfos)
			.HasForeignKey(e => e.StoreInfoId)
            .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
