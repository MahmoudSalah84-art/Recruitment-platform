using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository.IRepo
{
	public interface IApplicationRepository : IRepository<JobApplication>
	{
		Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId);

		Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(JobApplication application, CancellationToken ct = default);
		Task UpdateAsync(JobApplication application, CancellationToken ct = default);
		Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId, CancellationToken ct = default);
	}
}
