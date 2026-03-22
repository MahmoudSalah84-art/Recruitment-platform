using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IFileService
	{
		Task<string> UploadImageAsync(FileUploadDto file);
		Task<string> UploadFileAsync(FileUploadDto file);
	}
}
