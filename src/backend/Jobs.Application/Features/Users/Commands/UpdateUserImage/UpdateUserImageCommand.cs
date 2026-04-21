using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Users.Commands.UpdateUserImage
{
	public record UpdateUserImageCommand(
	string UserId,
	FileUploadDto file) : ICommand;
}
