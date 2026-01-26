using Jobs.Application.Abstractions.Messaging;
using MediatR;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{
	public record WithdrawApplicationCommand(Guid ApplicationId) : ICommand;
}