
namespace Jobs.Application.Common.DTOs
{
	//for all stream file
	public record FileUploadDto(
	string FileName,
	string ContentType,
	Stream Content
	);
}
