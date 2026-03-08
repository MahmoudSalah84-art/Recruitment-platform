using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepositories
{
	public interface IUnitOfWork
	{
		// Repositories
		IJobRepository Jobs { get; }
		IUserRepository Users { get; }
		ICompanyRepository Companies { get; }
		IApplicationRepository Applications { get; }
		ICVRepository CVs { get; }
		ISkillRepository Skills { get; }
		IJobSkillRepository JobSkills { get; }
		IUserSkillRepository UserSkills { get; }

		// Save changes
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		int SaveChanges();
	}
}
