using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<CVJobRecommendation>> RecommendCvsToJobAsync(Guid jobId, int maxResults);
    }
}
