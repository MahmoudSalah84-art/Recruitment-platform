using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Repository.Repo
{
	public interface IApplicationRepository : IRepository<JobApplication>
	{
		Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId, CancellationToken ct = default);
	}
}
