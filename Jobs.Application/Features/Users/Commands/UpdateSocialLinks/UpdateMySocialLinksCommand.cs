using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Commands.UpdateSocialLinks
{
	public record UpdateMySocialLinksCommand(
	string? LinkedInUrl,
	string? GitHubUrl,
	string? PortfolioUrl
	) : ICommand;

}
