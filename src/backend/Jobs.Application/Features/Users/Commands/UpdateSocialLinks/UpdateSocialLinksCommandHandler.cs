using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Users.Commands.UpdateSocialLinks
{
	public class UpdateMySocialLinksCommandHandler : ICommandHandler<UpdateMySocialLinksCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public UpdateMySocialLinksCommandHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle( UpdateMySocialLinksCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users
				.GetByIdAsync(_currentUser.UserId, cancellationToken);

			if (user is null)
				return Result.Failure("User not found");

			user.UpdateSocialLinks(
				request.LinkedInUrl,
				request.GitHubUrl,
				request.PortfolioUrl
			);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}

}
