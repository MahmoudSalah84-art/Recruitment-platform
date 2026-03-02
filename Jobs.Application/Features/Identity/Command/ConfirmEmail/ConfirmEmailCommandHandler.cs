using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.ConfirmEmail
{
	public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
	{
		private readonly IIdentityService _svc;
		public ConfirmEmailCommandHandler(IIdentityService svc) => _svc = svc;


		public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
		{
			return await _svc.ConfirmEmailAsync(request.UserId, request.Token);
		}
	}
}