using FluentValidation;

namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public class UpdateCompanyLogoCommandValidator : AbstractValidator<UpdateCompanyLogoCommand>
	{
		public UpdateCompanyLogoCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty();

			RuleFor(x => x.file.ContentType)
				.NotEmpty()
				.Must(BeValidImageType)
				.WithMessage("Only jpg, jpeg, png images are allowed");
		}

		private bool BeValidImageType(string contentType)
		{
			return contentType == "image/jpeg"
				|| contentType == "image/png"
				|| contentType == "image/jpg";
		}
	}
}
