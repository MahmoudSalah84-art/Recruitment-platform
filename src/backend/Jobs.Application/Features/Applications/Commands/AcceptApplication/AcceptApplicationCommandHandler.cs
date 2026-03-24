using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Commands.AcceptApplication
{
	public class AcceptApplicationCommandHandler : ICommandHandler<AcceptApplicationCommand >
	{
		private readonly IUnitOfWork _unitOfWork;

		public AcceptApplicationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle( AcceptApplicationCommand request, CancellationToken cancellationToken)
		{
			var application = await _unitOfWork.Applications
				.GetByIdAsync(request.ApplicationId, cancellationToken);

			if (application is null)
				return Result.Failure( "Application not found.");

			application.ChangeStatus(ApplicationStatus.Accepted);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
