using Jobs.Domain.IRepository;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs.Infrastructure.Repositories.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly JobDbContext _context;
		private readonly IServiceProvider _serviceProvider;
		private readonly Dictionary<Type, object> _repositories = new();

		public UnitOfWork(JobDbContext context, IServiceProvider serviceProvider)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_serviceProvider = serviceProvider;
		}


		public IJobRepository Jobs => GetRepository<IJobRepository>();
		public IUserRepository Users => GetRepository<IUserRepository>();
		public ICompanyRepository Companies => GetRepository<ICompanyRepository>();
		public IApplicationRepository Applications => GetRepository<IApplicationRepository>();
		public ICVRepository CVs => GetRepository<ICVRepository>();

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
			=> await _context.SaveChangesAsync(cancellationToken);

		public int SaveChanges()
			=> _context.SaveChanges();

		public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
			=> await _context.Database.BeginTransactionAsync(cancellationToken);

		private T GetRepository<T>() where T : notnull
		{
			if (_repositories.ContainsKey(typeof(T)))
				return (T)_repositories[typeof(T)];

			var repository = _serviceProvider.GetRequiredService<T>();
			if (repository is null)
				throw new InvalidOperationException($"Repository of type {typeof(T).Name} could not be resolved.");

			_repositories.Add(typeof(T), repository);
			return repository;
		}
	}
}
