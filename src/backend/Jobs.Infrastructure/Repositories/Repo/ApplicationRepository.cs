using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class ApplicationRepository : Repository<JobApplication>, IApplicationRepository
	{
        public ApplicationRepository(JobDbContext context) : base(context) { }


		/// <summary>
		/// Checks if an applicant has applied for a specific job.
		/// </summary>
		/// <param name="jobId">The ID of the job to check.</param>
		/// <param name="applicantId">The ID of the applicant.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>True if the applicant has applied for the job, otherwise false.</returns>
		public async Task<bool> ExistsForApplicantAsync(string jobId, string applicantId, CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()
				.AnyAsync(ja => ja.JobId == jobId && ja.ApplicantId == applicantId, ct);
		}
	}
}
