
namespace Jobs.Application.Features.Companies.Command.Register
{
	using FluentValidation;
    using global::Jobs.Application.Common.DTOs;

	namespace Jobs.Application.Features.Companies.Command.Register
	{
		public class RegisterCompanyCommandValidator : AbstractValidator<RegisterCompanyCommand>
		{
			public RegisterCompanyCommandValidator()
			{
				RuleFor(x => x.UserName)
					.NotEmpty()
					.WithMessage("Company name is required")
					.MaximumLength(100);

				RuleFor(x => x.Email)
					.NotEmpty()
					.WithMessage("Email is required")
					.EmailAddress()
					.WithMessage("Invalid email format");

				RuleFor(x => x.Industry)
					.NotEmpty()
					.WithMessage("Industry is required");

				RuleFor(x => x.Country)
					.NotEmpty()
					.WithMessage("Country is required");

				RuleFor(x => x.City)
					.NotEmpty()
					.WithMessage("City is required");

				RuleFor(x => x.Street)
					.NotEmpty()
					.WithMessage("Street is required");

				RuleFor(x => x.BuildingNumber)
					.NotEmpty()
					.WithMessage("Building number is required");

				RuleFor(x => x.PostalCode)
					.NotEmpty()
					.WithMessage("Postal code is required");

				RuleFor(x => x.Description)
					.MaximumLength(500);

				RuleFor(x => x.Image)
					.SetValidator(new FileUploadDtoValidator())
					.When(x => x.Image != null);

				RuleFor(x => x.Password)
					.NotEmpty()
					.WithMessage("Password is required")
					.MinimumLength(8)
					.WithMessage("Password must be at least 8 characters")
					.Matches("[A-Z]").WithMessage("Password must contain uppercase letter")
					.Matches("[a-z]").WithMessage("Password must contain lowercase letter")
					.Matches("[0-9]").WithMessage("Password must contain a number");

				RuleFor(x => x.ConfirmPassword)
					.Equal(x => x.Password)
					.WithMessage("Passwords do not match");
			}
		}
	}
}
