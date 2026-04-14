using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
 
namespace Jobs.Application.Features.Companies.Command.AddEmployee
{
	public class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddEmployeeCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure( "Company not found.");

			var employee = await _unitOfWork.Users.GetByIdAsync(request.EmployeeId, cancellationToken);
			if (employee is null)
				return Result.Failure( "User not found.");

			company.AddEmployee(employee);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}