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
	public class UnitInfoConfiguration : IEntityTypeConfiguration<UnitInfo>
	{
		public void Configure(EntityTypeBuilder<UnitInfo> builder)
		{
			builder.Property(e => e.UnitName)
				.HasMaxLength(150)
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

			builder.HasMany(e => e.ProductInfos)
			.WithOne(pt => pt.UnitInfo)
			.HasForeignKey(e => e.UnitInfoId);

			builder.HasMany(e => e.TransactionInfos)
			.WithOne(pt => pt.UnitInfo)
			.HasForeignKey(e => e.UnitInfoId);

		}
	}
}
