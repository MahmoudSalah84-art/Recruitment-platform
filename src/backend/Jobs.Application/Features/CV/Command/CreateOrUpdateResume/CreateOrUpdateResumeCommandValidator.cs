using FluentValidation;
using Jobs.Application.Features.Companies.Command.UpdateCompanyLogo;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public class CreateOrUpdateResumeCommandValidator  : AbstractValidator<UpdateCompanyLogoCommand>
	{
		public CreateOrUpdateResumeCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty();

			RuleFor(x => x.file)
				.NotNull().WithMessage("File is required.");

			RuleFor(x => x.file.ContentType)
				.NotEmpty()
				.Must(BePdfFile)
				.WithMessage("Only PDF files are allowed.");

			RuleFor(x => x.file.FileName)
				.Must(BepdfFileByExtention);

		}
		private bool BepdfFileByExtention(string fileName) 
		{
			// Check extension
			var extension = Path.GetExtension(fileName).ToLower();
			if (extension != ".pdf")
				return false;

			return true;
		}

		private bool BePdfFile(string contentType)
		{
			if (contentType == null)
				return false;

			// Check content type (not 100% secure but useful)
			if (contentType != "application/pdf")
				return false;

			return true;
		}
	}
}
