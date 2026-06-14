

namespace Jobs.Application.Features.Applications.Queries.GetUseAppForSeekerById
{

	public record GetUserApplicationForSeekerDTO(
	string Id,
	string JobId,
	string JobTitle,
	int MatchScore,
	string Status
	);
}
