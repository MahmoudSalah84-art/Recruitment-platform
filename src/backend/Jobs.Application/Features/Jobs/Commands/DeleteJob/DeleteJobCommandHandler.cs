using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Jobs.Commands.DeleteJob
{
	public class DeleteJobCommandHandler : ICommandHandler<DeleteJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteJobCommandHandler(  IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result.Failure( "Job not found.");

			job.Delete();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
