using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Commands.DeleteSocialLink
{
	public record DeleteSocialLinkCommand(string Platform) : ICommand;

}
