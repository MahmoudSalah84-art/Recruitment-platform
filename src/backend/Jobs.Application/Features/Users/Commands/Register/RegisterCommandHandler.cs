using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.Users.Commands.Register;
using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Identity.Command.Register
{
	public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponse>
	{

		private readonly IIdentityService _identityService;
		private readonly IUnitOfWork _unitOfWork;

		public RegisterCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork)
		{
			_identityService = identityService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AuthResponse>> Handle(RegisterCommand cmd, CancellationToken ct)
		{
			var user = new User(
				firstName: cmd.FirstName,
				lastName: cmd.LastName,
				bio: cmd.Bio
			);


			var registerRequest = new RegisterRequest(
				Id: user.Id,
				FirstName: cmd.FirstName,
				LastName: cmd.LastName,
				UserName: cmd.Email,
				Email: cmd.Email,
				Password: cmd.Password,
				ConfirmPassword: cmd.ConfirmPassword,
				Roles.User
			);

			var result = await _identityService.RegisterAsync(registerRequest);
			if(!result.IsSuccess)
			{
				return Result<AuthResponse>.Failure(result.Error);
			}

			_unitOfWork.Users.Add(user);
			await _unitOfWork.SaveChangesAsync(ct);

			return result;
		}
	}
}
