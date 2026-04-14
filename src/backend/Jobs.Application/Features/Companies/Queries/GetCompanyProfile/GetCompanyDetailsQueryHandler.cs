using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Queries.GetCompanyProfile;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyDetails
{
	public class GetCompanyProfileQueryHandler : IQueryHandler<GetCompanyProfileQuery, CompanyProfileDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCompanyProfileQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CompanyProfileDto>> Handle( GetCompanyProfileQuery request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId);

			if (company is null)
				return Result<CompanyProfileDto>.Failure("Company not found");

			var dto = new CompanyProfileDto(
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

			return Result<CompanyProfileDto>.Success(dto);
		}
	}
}