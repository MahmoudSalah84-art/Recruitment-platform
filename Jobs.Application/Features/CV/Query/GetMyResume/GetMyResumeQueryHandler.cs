using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Application.Features.CV.Query.GetMyResume
{
	public class GetMyResumeQueryHandler : IQueryHandler<GetMyResumeQuery, UserResumeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public GetMyResumeQueryHandler(
			IUnitOfWork unitOfWork,
			ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result<UserResumeDto>> Handle(GetMyResumeQuery request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == Guid.Empty)
				return Result<UserResumeDto>.Failure("User not authenticated");

			var resume = await _unitOfWork.CVs.Query()
				.Where(c => c.UserId == _currentUser.UserId)
				.Select(c => new UserResumeDto
				{
					Title = c.Title,
					FilePath = c.FilePath,
					Summary = c.SummaryText
				})
				.FirstOrDefaultAsync(cancellationToken);

			if (resume is null)
				return Result<UserResumeDto>.Failure("Resume not found");

			return Result<UserResumeDto>.Success(resume);
		}
	}
}
