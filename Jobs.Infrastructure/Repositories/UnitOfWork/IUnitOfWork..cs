using Jobs.Domain.IRepository;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.Repo;
using Microsoft.EntityFrameworkCore.Storage;

namespace Jobs.Infrastructure.Repositories.UnitOfWork
{
	public interface IUnitOfWork 
	{
		IJobRepository Jobs { get; }
		IApplicationRepository Applications { get; }
		ICompanyRepository Companies { get; }
		ICVRepository CVs { get; }
		IUserRepository Users { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
	}
}
