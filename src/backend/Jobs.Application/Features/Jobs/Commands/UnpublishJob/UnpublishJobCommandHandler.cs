using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobs.Application.Features.Jobs.Commands.UnpublishJob
{
	public class UnpublishJobCommandHandler : ICommandHandler<UnpublishJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UnpublishJobCommandHandler(  IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UnpublishJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result.Failure("Job not found.");

			job.Unpublish();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
