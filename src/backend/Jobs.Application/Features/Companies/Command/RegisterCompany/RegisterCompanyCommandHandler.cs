using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Companies.Command.Register
{
	public class RegisterCompanyHandler : ICommandHandler<RegisterCompanyCommand, AuthResponse>
	{
		private readonly IIdentityService _identityService;
		private readonly IUnitOfWork _unitOfWork;

		public RegisterCompanyHandler(IIdentityService identityService, IUnitOfWork unitOfWork)
		{
			_identityService = identityService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AuthResponse>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
		{
			var company = new Company(
				name: request.UserName,
				industry: request.Industry,
				Country: request.Country,
				city: request.City,
				Street: request.Street,
				BuildingNumber: request.BuildingNumber,
				postalCode: request.PostalCode,
				logoUrl: null,
				description: request.Description
			);


			var registerRequest = new RegisterRequest(
				Id: company.Id,
				FirstName: string.Empty,
				LastName: string.Empty,
				UserName: request.UserName,
				Email: request.Email,
				Password: request.Password,
				ConfirmPassword: request.ConfirmPassword,
				Roles.Company
			);

			var result = await _identityService.RegisterAsync(registerRequest);
			if (!result.IsSuccess)
				return Result<AuthResponse>.Failure(result.Error);	


			_unitOfWork.Companies.Add(company);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return result;
		}
	}
}








//public Company(string name, Func<string, bool> nameExists,
//			string email, Func<string, bool> emailExists,
//			string industry, string Country, string city,
//			string Street, string BuildingNumber, string postalCode,
//			string? logoUrl = default, string? description = default)