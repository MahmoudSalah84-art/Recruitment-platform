using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Jobs.Commands.PublishJob
{
	public class PublishJobCommandHandler : ICommandHandler<PublishJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public PublishJobCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(PublishJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdWithSkillsAsync(request.JobId, cancellationToken);


			if (job is null)
				return Result.Failure("Job not found.");

			job.Publish();

			var jj = job;
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
