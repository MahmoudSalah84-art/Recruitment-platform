using Hangfire;
using MediatR;

namespace Jobs.Infrastructure.BackgroundJobs
{
	public sealed class BackgroundJobMediator
	{
		private readonly ISender _sender;

		public BackgroundJobMediator(ISender sender)
		{
			_sender = sender;
		}

		[AutomaticRetry(Attempts = 3)]
		public async Task SendAsync<TCommand>(
			TCommand command,
			CancellationToken cancellationToken)
			where TCommand : IBaseRequest
		{
			await _sender.Send(command, cancellationToken);
		}
	}
}
