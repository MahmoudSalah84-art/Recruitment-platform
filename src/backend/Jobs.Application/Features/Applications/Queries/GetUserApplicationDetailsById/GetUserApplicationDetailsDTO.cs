
namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{

	public record GetUserApplicationDetailsDTO(
	string Id,
	string ApplicantId,
	string ApplicantName,
	string JobId,
	string JobTitle,
	string? CvId,
	int MatchScore,
	string Status,
	string CompanyId,
	string CompanyName
	);
}
