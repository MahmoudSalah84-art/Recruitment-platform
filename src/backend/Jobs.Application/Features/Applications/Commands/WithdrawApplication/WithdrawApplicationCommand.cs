using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{
	public record WithdrawApplicationCommand(string ApplicantId,string ApplicationId) : ICommand;
}