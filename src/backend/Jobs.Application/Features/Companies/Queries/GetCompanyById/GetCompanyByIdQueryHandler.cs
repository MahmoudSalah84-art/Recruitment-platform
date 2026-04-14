using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyById
{
	public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery,CompanyResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CompanyResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result<CompanyResponse>.Failure( "Company not found.");

			var response = new CompanyResponse(
				company.Id,
				company.Name,
				company.Email.Value,
				company.Industry,
				company.CompanyAddress.Street,
				company.CompanyAddress.City,
				company.CompanyAddress.Country,
				company.EmployeesCount,
				company.Description,
				company.LogoUrl,
				company.Jobs.Count(j => j.IsPublished && !j.IsExpired),
				company.Employees.Count
			);

			return Result<CompanyResponse>.Success(response);
		}
	}
}
