using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Queries.GetCompanies;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyById
{
	public sealed class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CompanyDto>> Handle( GetCompanyByIdQuery request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);

			if (company is null)
				return Result<CompanyDto>.Failure("Company not found");

			var dto = new CompanyDto(
					company.Id,
					company.Name,
					company.Description,
					company.Industry,
					company.EmployeesCount,
					company.LogoUrl,

					company.CompanyAddress.Country,
					company.CompanyAddress.City,
					company.CompanyAddress.Street,
					company.CompanyAddress.BuildingNumber,
					company.CompanyAddress.PostalCode
			);

			return Result<CompanyDto>.Success(dto);
		}
    }
}