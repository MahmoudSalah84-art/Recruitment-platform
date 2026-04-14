using Hangfire;
using Jobs.Application.Abstractions.Interfaces;
using MediatR;

namespace Jobs.Infrastructure.BackgroundJobs
{
	internal sealed class HangfireBackgroundJobService : IBackgroundJobService
	{
		public void Enqueue<TCommand>(TCommand command) where TCommand : IBaseRequest
		{
			BackgroundJob.Enqueue<BackgroundJobMediator>(
				x => x.SendAsync(command, CancellationToken.None));
		}
	}
}