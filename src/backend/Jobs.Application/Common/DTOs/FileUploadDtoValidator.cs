using FluentValidation;

namespace Jobs.Application.Common.DTOs
{
	public class FileUploadDtoValidator : AbstractValidator<FileUploadDto?>
	{
		private const long MaxFileSize = 2 * 1024 * 1024; // 2MB

		public FileUploadDtoValidator()
		{
			RuleFor(x => x.FileName)
				.NotEmpty()
				.WithMessage("File name is required");


			RuleFor(x => x.Content)
				.NotNull()
				.Must(stream => stream.Length > 0)
				.WithMessage("File cannot be empty")
				.Must(stream => stream.Length <= MaxFileSize)
				.WithMessage("File size must be less than 2MB");
		}

		
	}
}
