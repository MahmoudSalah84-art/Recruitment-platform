using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Users.Commands.AddSocialLink
{
	public class AddSocialLinkCommandHandler : ICommandHandler<AddSocialLinkCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public AddSocialLinkCommandHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle( AddSocialLinkCommand request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == Guid.Empty)
				return Result.Failure("User not authenticated");

			if (string.IsNullOrWhiteSpace(request.Platform) ||
				string.IsNullOrWhiteSpace(request.Url))
				return Result.Failure("Invalid social link data");

			var link = UserSocialLink.Create(
				_currentUser.UserId,
				request.Platform,
				request.Url
			);

			_unitOfWork.UserSocialLinks.Add(link);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}

}
