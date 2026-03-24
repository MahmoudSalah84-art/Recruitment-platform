using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{
	public class WithdrawApplicationCommandHandler : ICommandHandler<WithdrawApplicationCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public WithdrawApplicationCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
		{

			var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationId, cancellationToken);

			if (application is null)
				return Result.Failure("Application not found.");

			//if (application.ApplicantId != userId)
			//	return Result<Unit>.Failure("you don't have athourization for this application.");

			if (application.Status != ApplicationStatus.Pending)
				return Result.Failure("Cannot withdraw an application that has already been processed.");

			_unitOfWork.Applications.Remove(application);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}