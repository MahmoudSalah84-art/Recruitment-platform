using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.User;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyEmployees
{
	public class GetCompanyEmployeesQueryHandler
	: IQueryHandler<GetCompanyEmployeesQuery, PaginatedList<EmployeeResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCompanyEmployeesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<EmployeeResponse>>> Handle(
			GetCompanyEmployeesQuery request,
			CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result<PaginatedList<EmployeeResponse>>.Failure( "Company not found.");

			var spec = new GetCompanyEmployeesSpecification(request.CompanyId, request.PageSize, request.Page);

			var totalCount = await _unitOfWork.Users.CountAsync(spec);
			var result = await _unitOfWork.Users.ListWithSpecAsync(spec);
			 

			var response = result
				.Select(e => new EmployeeResponse(e.Id, e.FirstName, e.LastName, e.Email.Value))
				.ToList();

			var paginatedList = PaginatedList<EmployeeResponse>.Create(response, totalCount, request.Page, request.PageSize);

			return Result<PaginatedList<EmployeeResponse>>.Success(paginatedList);

		}
	}
}
