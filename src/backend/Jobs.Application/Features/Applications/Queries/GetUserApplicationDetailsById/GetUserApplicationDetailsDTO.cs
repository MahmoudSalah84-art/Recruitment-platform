
using Jobs.Domain.ValueObjects;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{

	public record GetUserApplicationDetailsDTO(
	string Id,
	string ApplicantId,
	string ApplicantName,
	string Email,
	string JobTitle,
	string? CvPath,
	int MatchScore,
	string Status
	);
}
