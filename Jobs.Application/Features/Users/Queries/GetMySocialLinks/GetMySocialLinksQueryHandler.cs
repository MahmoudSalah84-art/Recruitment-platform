using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Application.Features.Users.Queries.GetMySocialLinks
{
	public class GetMySocialLinksQueryHandler : IQueryHandler<GetMySocialLinksQuery, SocialLinksDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public GetMySocialLinksQueryHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result<SocialLinksDto>> Handle( GetMySocialLinksQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.Query()
				.Where(x => x.Id == _currentUser.UserId)
				.Select(x => new SocialLinksDto(
					x.LinkedInUrl,
					x.GitHubUrl,
					x.PortfolioUrl)
				)
				.FirstOrDefaultAsync(cancellationToken);

			if (user is null)
				return Result<SocialLinksDto>.Failure("User not found");

			return Result<SocialLinksDto>.Success(user);
		}
	}
}
