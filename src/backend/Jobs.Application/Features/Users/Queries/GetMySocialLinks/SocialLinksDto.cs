
namespace Jobs.Application.Features.Users.Queries.GetMySocialLinks
{
	public record SocialLinksDto(
	string? LinkedInUrl,
	string? GitHubUrl,
	string? PortfolioUrl
	);

}
