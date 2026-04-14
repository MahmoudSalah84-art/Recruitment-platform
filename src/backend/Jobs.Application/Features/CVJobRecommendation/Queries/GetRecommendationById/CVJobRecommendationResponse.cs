using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById
{
	public record CVJobRecommendationResponse(
	string Id,
	string CvId,
	string JobId,
	string JobTitle,
	string CompanyId,
	int Score,
	bool IsActive,
	DateTime? DeactivatedAt);
}
