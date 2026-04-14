using Jobs.Domain.Entities;


namespace Jobs.Domain.IRepositories
{
	public interface IApplicationRepository : IRepository<JobApplication>
	{
		/// <summary>
		/// Checks if an applicant has applied for a specific job.
		/// </summary>
		/// <param name="jobId">The ID of the job to check.</param>
		/// <param name="applicantId">The ID of the applicant.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>True if the applicant has applied for the job, otherwise false.</returns>
		Task<bool> ExistsForApplicantAsync(string jobId, string applicantId, CancellationToken ct = default);

	}
}
