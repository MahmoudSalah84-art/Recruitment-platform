


using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Jobs.Commands.DeleteJob
{
	public record DeleteJobCommand(string CompanyId,string JobId) : ICommand;
}
