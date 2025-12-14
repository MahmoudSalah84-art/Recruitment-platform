using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
	public interface IApplicationRepository
	{
		Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(JobApplication application, CancellationToken ct = default);
		Task UpdateAsync(JobApplication application, CancellationToken ct = default);
		Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId, CancellationToken ct = default);
	}
}


