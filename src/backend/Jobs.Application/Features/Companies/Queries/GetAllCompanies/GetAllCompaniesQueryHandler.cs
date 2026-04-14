using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Company;

namespace Jobs.Application.Features.Companies.Queries.GetAllCompanies
{
	public class GetAllCompaniesQueryHandler
	: IQueryHandler<GetAllCompaniesQuery, PaginatedList<CompanyResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<CompanyResponse>>> Handle(
			GetAllCompaniesQuery request,
			CancellationToken cancellationToken)
		{

			var spec = new CommpaniesWithDetilesSpecification(
				request.Name,
				request.Industry,
				request.Page,
				request.PageSize);

			var result = await _unitOfWork.Companies.ListWithSpecAsync(spec);
			var totalCount = await _unitOfWork.Companies.CountAsync(spec);


			var response = result
				.Select(c => new CompanyResponse(
					c.Id,
					c.Name,
					c.Email.Value,
					c.Industry,
					c.CompanyAddress.Street,
					c.CompanyAddress.City,
					c.CompanyAddress.Country,
					c.EmployeesCount,
					c.Description,
					c.LogoUrl,
					c.Jobs.Count(j => j.IsPublished && !j.IsExpired),
					c.Employees.Count))
				.ToList();

			var paginatedList = PaginatedList<CompanyResponse>.Create(response, totalCount, request.Page, request.PageSize);

			return Result<PaginatedList<CompanyResponse>>.Success(paginatedList);
		}
	}
}