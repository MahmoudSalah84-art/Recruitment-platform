using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Commands.RejectApplication
{
	public class RejectApplicationCommandHandler : ICommandHandler<RejectApplicationCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public RejectApplicationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle( RejectApplicationCommand request, CancellationToken cancellationToken)
		{
			var application = await _unitOfWork.Applications
				.GetByIdAsync(request.ApplicationId, cancellationToken);

			if (application is null)
				return Result.Failure( "Application not found.");

			application.UpdateStatus(ApplicationStatus.Rejected);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
