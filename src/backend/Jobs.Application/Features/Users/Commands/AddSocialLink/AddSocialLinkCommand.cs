using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Commands.AddSocialLink
{
	public record AddSocialLinkCommand(
	string Platform,
	string Url
	) : ICommand;

}
