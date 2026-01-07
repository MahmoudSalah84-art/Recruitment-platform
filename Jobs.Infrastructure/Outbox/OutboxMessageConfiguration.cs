using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Outbox
{
	public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			builder.ToTable("OutboxMessages");

			builder.HasKey(o => o.Id);
			builder.Property(o => o.Type)
				   .HasMaxLength(500)
				   .IsRequired();
			builder.Property(o => o.Content)
				   .HasColumnType("nvarchar(max)")
				   .IsRequired();
			builder.Property(o => o.OccurredOn)
				   .HasDefaultValueSql("GETUTCDATE()");
			builder.Property(o => o.Processed)
				   .HasDefaultValue(false);
			builder.Property(o => o.Error)
				   .HasColumnType("nvarchar(400)");
		}
	}
}
