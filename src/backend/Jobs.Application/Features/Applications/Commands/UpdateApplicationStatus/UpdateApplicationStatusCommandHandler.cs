using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Applications.Commands.UpdateApplicationStatus
{
	public class UpdateApplicationStatusCommandHandler
	: ICommandHandler<UpdateApplicationStatusCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateApplicationStatusCommandHandler(
			ICompanyRepository companyRepository,
			IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(
			UpdateApplicationStatusCommand request,
			CancellationToken cancellationToken)
		{
			// 1. Get the application
			var application = await _unitOfWork.Applications .GetByIdAsync(request.ApplicationId, cancellationToken);
			if (application is null)
				return Result.Failure("Job Application not found");
  
			var Job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);

			if (Job!.CompanyId != request.CompanyId)
				return Result.Failure("Unauthorized to update this application");

			// 3. Update status via domain method
			application.UpdateStatus(request.NewStatus);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
