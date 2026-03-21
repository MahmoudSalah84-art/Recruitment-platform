using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Companies.Command.RemoveEmployee
{
	public class RemoveEmployeeCommandHandler : ICommandHandler<RemoveEmployeeCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public RemoveEmployeeCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(RemoveEmployeeCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdWithEmployeeAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure("Company not found.");

			company.RemoveEmployee(request.EmployeeId);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
