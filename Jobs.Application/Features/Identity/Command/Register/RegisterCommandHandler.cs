using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.Register
{
	public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponse>
	{
		private readonly IIdentityService _svc;
		public RegisterCommandHandler(IIdentityService svc) => _svc = svc;


		public async Task<Result<AuthResponse>> Handle(RegisterCommand cmd, CancellationToken ct)
			=> await _svc.RegisterAsync(new RegisterRequest( cmd.FirstName, cmd.LastName,cmd.Email, cmd.Password, cmd.ConfirmPassword));
	}

}
