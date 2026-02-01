using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.ValueObjects;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public record CreateOrUpdateResumeCommand(
	string Title,
	FilePath FilePath,
	string Summary
	) : ICommand;

}
