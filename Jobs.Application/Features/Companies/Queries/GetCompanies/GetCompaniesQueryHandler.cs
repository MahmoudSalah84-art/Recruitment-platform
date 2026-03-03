using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Application.Features.Companies.Queries.GetCompanies
{
	public sealed class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, List<CompanyDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCompaniesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<CompanyDto>>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
		{
			var companies = await _unitOfWork.Companies.Query()
				.Select(c => new CompanyDto
				(
					c.Id,
					c.Name,
					c.Description,
					c.Industry,
					c.EmployeesCount,
					c.LogoUrl,

					c.CompanyAddress.Country,
					c.CompanyAddress.City,
					c.CompanyAddress.Street,
					c.CompanyAddress.BuildingNumber,
					c.CompanyAddress.PostalCode
				))
				.ToListAsync(cancellationToken);

			return Result<List<CompanyDto>>.Success(companies);	
		}
    }
}