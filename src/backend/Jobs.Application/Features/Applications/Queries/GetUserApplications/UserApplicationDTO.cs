
namespace Jobs.Application.Features.Applications.Queries.GetUserApplications
{
    public record UserApplicationDTO(
	string Id, 
	string JobId, 
	string CompanyName, 
	int MatchScore,
	string Status,
	DateTime Created)
	{}
}
