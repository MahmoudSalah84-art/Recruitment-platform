using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Commands.UpdateSinglePlatform
{
	public record UpdateSingleSocialLinkCommand(
	string Platform,
	string Url
	) : ICommand;
}
