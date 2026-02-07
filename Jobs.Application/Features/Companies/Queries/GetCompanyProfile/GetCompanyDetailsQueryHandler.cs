using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Interfaces;
using Jobs.Application.Features.Companies.Queries.GetCompanyProfile;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
			if (_currentUser.UserId == Guid.Empty)
				return Result<CompanyProfileDto>.Failure("User not authenticated");

			var company = await _unitOfWork.Companies.Query()
				.Where(x => x.Id == _currentUser.UserId)
				.Select(x => new CompanyProfileDto(
					x.Id,
					x.Name,
					x.Description,
					x.Industry,
					x.EmployeesCount,
					x.LogoUrl,

					x.CompanyAddress.Country,
					x.CompanyAddress.City,
					x.CompanyAddress.Street,
					x.CompanyAddress.BuildingNumber,
					x.CompanyAddress.PostalCode
				))
				.FirstOrDefaultAsync(cancellationToken);

			if (company is null)
				return Result<CompanyProfileDto>.Failure("Company not found");

			return Result<CompanyProfileDto>.Success(company);
		}
	}
}
