using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class CVJobRecommendationRepository : Repository<CVJobRecommendation> ,  ICVJobRecommendationRepository 
	{
		public CVJobRecommendationRepository(JobDbContext context) : base(context) { }



		public async Task<bool> ExistsByCvAndJobAsync( string cvId, string jobId, CancellationToken cancellationToken)
		{
			return await _set.AnyAsync(r => r.CvId == cvId && r.JobId == jobId, cancellationToken);
		}

		public async Task<CVJobRecommendation?> GetByIdWithDetailsAsync( string id, CancellationToken cancellationToken)
		{
			return await _set
				.Include(r => r.Job)
				.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
		}

		public async Task AddRangeAsync(IEnumerable<CVJobRecommendation> recommendations, CancellationToken cancellationToken)
		{
			await _set.AddRangeAsync(recommendations, cancellationToken);
		}

		public async Task<CVJobRecommendation?> GetByCvIdAndJobIdAsync(string cvId, string jobId, CancellationToken cancellationToken)
		{
  			return await _set.FirstOrDefaultAsync(r => r.CvId == cvId && r.JobId == jobId, cancellationToken);
		}

		public async Task<IEnumerable<CVJobRecommendation>> GetByCvIdAsync(string cvId, CancellationToken cancellationToken)
		{
			return await _set.Where(r => r.CvId == cvId).ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<CVJobRecommendation>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken)
		{
			return await _set.Where(r => r.JobId == jobId).ToListAsync(cancellationToken);
		}
	}
}