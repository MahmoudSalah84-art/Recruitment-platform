using Jobs.Domain.Entities;
using Jobs.Domain.Interfaces;
using Jobs.Domain.IRepository.IRepo;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Jobs.Infrastructure.Repositories.Repo
{
	public class ApplicationRepository : BaseRepository<JobApplication>, IApplicationRepository
	{
		public ApplicationRepository(JobDbContext context) : base(context)
		{
		}

        public Task AddAsync(JobApplication application, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId)
		{
			return await _set.AsNoTracking().AnyAsync(a => a.JobId == jobId && a.ApplicantId == applicantId);
		}

        public Task<bool> ExistsForApplicantAsync(Guid jobId, Guid applicantId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(JobApplication application, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
