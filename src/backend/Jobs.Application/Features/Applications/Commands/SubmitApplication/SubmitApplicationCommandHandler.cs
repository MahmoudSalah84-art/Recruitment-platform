using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Commands.SubmitApplication
{
	public class SubmitApplicationCommandHandler : ICommandHandler<SubmitApplicationCommand, string>
	{
		private readonly IUnitOfWork _unitOfWork;

		public SubmitApplicationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<string>> Handle( SubmitApplicationCommand request, CancellationToken cancellationToken)
		{
			var applicant = await _unitOfWork.Users.GetByIdAsync(request.ApplicantId, cancellationToken);
			if (applicant is null)
				return Result<string>.Failure( "Applicant not found.");

			// تتشيك إن الـ Job موجود ومنشور ومش expired
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);
			if (job is null)
				return Result<string>.Failure("Job not found.");

			if (!job.IsPublished)
				return Result<string>.Failure("Cannot apply for an unpublished job.");

			if (job.IsExpired)
				return Result<string>.Failure("Cannot apply for an expired job.");

			var alreadyApplied = await _unitOfWork.Applications
				.ExistsForApplicantAsync(request.ApplicantId, request.JobId, cancellationToken);

			if (alreadyApplied)
				return Result<string>.Failure("You have already applied for this job.");

			var application = new JobApplication(
				request.ApplicantId,
				request.JobId,
				request.MatchScore,
				request.CvId);

			 _unitOfWork.Applications.Add(application);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success( application.Id);
		}
	}
}