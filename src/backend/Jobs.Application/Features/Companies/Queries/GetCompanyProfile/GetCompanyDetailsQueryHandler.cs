using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Queries.GetCompanyProfile;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyDetails
{
	public class GetCompanyProfileQueryHandler : IQueryHandler<GetCompanyProfileQuery, CompanyProfileDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public GetCompanyProfileQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result<CompanyProfileDto>> Handle( GetCompanyProfileQuery request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == string.Empty)
				return Result<CompanyProfileDto>.Failure("User not authenticated");

			var company = await _unitOfWork.Companies.GetByIdAsync(_currentUser.UserId);

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