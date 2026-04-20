using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Domain.ValueObjects;


namespace Jobs.Application.Features.Jobs.Commands.CreateJob
{
	public class CreateJobCommandHandler : ICommandHandler<CreateJobCommand, string>
	{
	 
		private readonly IUnitOfWork _unitOfWork;

		public CreateJobCommandHandler( IUnitOfWork unitOfWork)
		{
			
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<string>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if(company == null)
			{
				return Result<string>.Failure($"Company with ID {request.CompanyId} not found.");
			}

			

			var job = new Job(
			request.CompanyId,
			request.Title,
			SalaryRange.Create(request.minSalary, request.maxSalary),
			request.Description,
			request.Requirements,
			request.EmploymentType,
			request.ExperienceLevel,
			request.ExpirationDate);

			_unitOfWork.Jobs.Add(job);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success(job.Id);
		}
	}
}