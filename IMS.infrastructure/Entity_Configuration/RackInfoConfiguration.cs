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
	public class RackInfoConfiguration : IEntityTypeConfiguration<RackInfo>
	{
		public void Configure(EntityTypeBuilder<RackInfo> builder)
		{
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

            builder.Property(e => e.RackName)
				.HasMaxLength(150)
				.IsUnicode(true);

			builder.Property(e => e.IsActive)
			.HasDefaultValue(true);

			builder.Property(e => e.CreatedDate)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()")
				.HasColumnType("datetime");

            builder.Property(e => e.CreatedBy)
				.IsRequired()
				.IsUnicode(true);

			builder.Property(e => e.ModifiedDate)
				.HasColumnType("datetime");

			builder.Property(e => e.ModifiedBy)
				.IsUnicode(true);

			builder.HasMany(e => e.ProductRateInfos)
			.WithOne(pt => pt.RackInfo)
			.HasForeignKey(e => e.RackInfoId);

			builder.HasOne(e => e.StoreInfo)
			.WithMany(pt => pt.RackInfos)
			.HasForeignKey(e => e.StoreInfoId)
            .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
