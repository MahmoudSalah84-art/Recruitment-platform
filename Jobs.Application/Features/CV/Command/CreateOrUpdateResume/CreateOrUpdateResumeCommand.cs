using Jobs.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public record CreateOrUpdateResumeCommand(
	string Title,
	IFormFile File,
	string Summary
	) : ICommand;

}
