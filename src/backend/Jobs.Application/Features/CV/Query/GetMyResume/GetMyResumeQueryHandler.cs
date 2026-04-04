using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CV.Query.GetMyResume
{
	public class GetMyResumeQueryHandler : IQueryHandler<GetMyResumeQuery, UserResumeDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetMyResumeQueryHandler( IUnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UserResumeDto>> Handle(GetMyResumeQuery request, CancellationToken cancellationToken)
		{
			var resume = await _unitOfWork.CVs.GetByUserId(request.userId);

			if (resume is null)
				return Result<UserResumeDto>.Failure("Resume not found");

			UserResumeDto result = new UserResumeDto
			{
				Title = resume.Title,
				FilePath = resume.FilePath,
				Summary = resume.SummaryText
			};

			return Result<UserResumeDto>.Success(result);
		}
	}
}
