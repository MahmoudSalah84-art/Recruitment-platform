using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using MediatR;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public record CreateOrUpdateResumeCommand(
	string Title,
	FilePath FilePath,
	string Summary,
	ParsedData? ParsedData
	) : ICommand;

}
