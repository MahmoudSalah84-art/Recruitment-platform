using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Users.Commands.UpdateSinglePlatform
{
	public class UpdateSingleSocialLinkCommandHandler : ICommandHandler<UpdateSingleSocialLinkCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public async Task<Result> Handle( UpdateSingleSocialLinkCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users
				.GetByIdAsync(_currentUser.UserId, cancellationToken);

			if (user is null)
				return Result.Failure("User not found");

			user.UpdateSocialLink(request.Platform, request.Url);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}

}
