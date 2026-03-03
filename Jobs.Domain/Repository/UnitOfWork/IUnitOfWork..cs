using Jobs.Domain.Repository.Repo;

namespace Jobs.Domain.Repositories.UnitOfWork
{
	public interface IUnitOfWork 
	{
		IJobRepository Jobs { get; }
		IApplicationRepository Applications { get; }
		ICompanyRepository Companies { get; }
		ICVRepository CVs { get; }
		IUserRepository Users { get; }
		IExperienceRepository Experiences { get; }
		ISkillRepository Skills { get; }
		IUserSkillRepository UserSkills { get; }
		IJobSkillRepository JobSkills { get; }


		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
