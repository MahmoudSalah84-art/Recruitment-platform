using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using System.Transactions;


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
				isNameExists: await _unitOfWork.Companies.ExistsAsync(c => c.Name == request.UserName),
				email: request.Email,
				isEmailExists: await _unitOfWork.Companies.ExistsAsync(c => c.Email.Value == request.Email),
				industry: request.Industry,
				Country: request.Country,
				city: request.City,
				Street: request.Street,
				BuildingNumber: request.BuildingNumber,
				postalCode: request.PostalCode,
				logoUrl: null,
				description: request.Description
			);

			_unitOfWork.Companies.Add(company);
			await _unitOfWork.SaveChangesAsync(cancellationToken);


			var registerRequest = new RegisterRequest(
				Id: company.Id,
				FirstName: string.Empty,
				LastName: string.Empty,
				UserName: request.UserName,
				Email: request.Email,
				Password: request.Password,
				ConfirmPassword: request.ConfirmPassword
			);

			return await _identityService.RegisterAsync(registerRequest);
		}
	}
}








//public Company(string name, Func<string, bool> nameExists,
//			string email, Func<string, bool> emailExists,
//			string industry, string Country, string city,
//			string Street, string BuildingNumber, string postalCode,
//			string? logoUrl = default, string? description = default)