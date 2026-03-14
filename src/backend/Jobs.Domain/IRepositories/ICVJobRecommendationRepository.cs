using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepositories
{
	public interface ICVJobRecommendationRepository : IRepository<CVJobRecommendation>
	{

		Task<bool> ExistsByCvAndJobAsync( string cvId, string jobId, CancellationToken cancellationToken);

		Task<CVJobRecommendation?> GetByIdWithDetailsAsync( string id, CancellationToken cancellationToken);
	}
}
