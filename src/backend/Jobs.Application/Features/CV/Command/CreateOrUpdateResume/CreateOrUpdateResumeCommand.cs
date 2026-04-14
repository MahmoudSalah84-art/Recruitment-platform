using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public record CreateOrUpdateResumeCommand(
	string UserId,
	FileUploadDto File ) : ICommand;
}
