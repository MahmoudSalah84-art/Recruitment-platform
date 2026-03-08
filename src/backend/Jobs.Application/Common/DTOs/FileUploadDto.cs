
namespace Jobs.Application.Common.DTOs
{
	public record FileUploadDto(
	string FileName,
	string ContentType,
	Stream Content
	);
}
