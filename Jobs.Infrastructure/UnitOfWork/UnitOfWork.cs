using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
	}

	public class UnitOfWork : IUnitOfWork
	{
		private readonly JobDbContext _context;

		public UnitOfWork(JobDbContext context) => _context = context;

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
			=> _context.SaveChangesAsync(cancellationToken);

		public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
			=> _context.Database.BeginTransactionAsync(cancellationToken);
	}
}
