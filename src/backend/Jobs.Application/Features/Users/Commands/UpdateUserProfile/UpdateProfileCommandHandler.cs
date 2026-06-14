using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using System.Security.Claims;

namespace Jobs.Application.Features.Users.Commands.UpdateUserProfile
{
	public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityService _identityService;


		public UpdateProfileCommandHandler(IUnitOfWork unitOfWork , IIdentityService identityService)
		{
			_unitOfWork = unitOfWork;
			_identityService = identityService;
		}

		public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
			if (user is null) return Result.Failure("unauthorized");


			user.UpdateProfile(
				request.FirstName,
				request.LastName,
				request.Bio
			);
			
			await _identityService.UpdateUserAsync(request.UserId, request.PhoneNumber, request.Email, request.UserName, request.FirstName, request.LastName);

			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
