using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository
{
	public interface IApplicationRepository : IRepository<JobApplication>
	{
		Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId, CancellationToken ct = default);
	}
}
