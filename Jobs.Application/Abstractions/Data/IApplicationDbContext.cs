using Microsoft.EntityFrameworkCore;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Outbox;

namespace Jobs.Application.Abstractions.Data
{
	public interface IApplicationDbContext
	{
		// الجداول الأساسية لموقع التوظيف
		DbSet<Job> Jobs { get; }
		DbSet<User> Users { get; }
		DbSet<OutboxMessage> OutboxMessages { get; }

		// أضف أي جداول أخرى تحتاجها مستقبلاً هنا
		// DbSet<Application> JobApplications { get; }

		// ميثود الحفظ التي سنناديها في الـ Handlers
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
