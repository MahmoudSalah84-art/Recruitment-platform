using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.Users.Commands.Register;
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
				email: cmd.Email,
				isEmailExists: await _unitOfWork.Companies.ExistsAsync(c => c.Email.Value == cmd.Email),
				phoneNumber: cmd.PhoneNumber,
				bio: cmd.Bio
			);


			_unitOfWork.Users.Add(user);
			await _unitOfWork.SaveChangesAsync(ct);

			var registerRequest = new RegisterRequest(
				Id: user.Id,
				FirstName: cmd.FirstName,
				LastName: cmd.LastName,
				UserName: cmd.Email,
				Email: cmd.Email,
				Password: cmd.Password,
				ConfirmPassword: cmd.ConfirmPassword
			);

			return await _identityService.RegisterAsync(registerRequest);
		}
	}
}
