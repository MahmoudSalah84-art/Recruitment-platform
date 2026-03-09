using FluentValidation;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Jobs.Commands.PublishJob
{
	public class PublishJobCommandValidator : AbstractValidator<PublishJobCommand>
	{
		public class PublishJobCommandHandler : ICommandHandler<PublishJobCommand>
		{
			private readonly IUnitOfWork _unitOfWork;

			public PublishJobCommandHandler(  IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<Result> Handle(PublishJobCommand request, CancellationToken cancellationToken)
			{
				var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

				if (job is null)
					return Result.Failure( "Job not found.");

				job.Publish();

				await _unitOfWork.SaveChangesAsync(cancellationToken);

				return Result.Success();
			}
		}
	}
}
