using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.DTOs;

namespace Jobs.Infrastructure.Services
{
	public class CloudinaryFileService : IFileService
	{
		private readonly Cloudinary _cloudinary;

		public CloudinaryFileService(Cloudinary cloudinary)
		{
			_cloudinary = cloudinary;
		}

		public async Task<string> UploadImageAsync(FileUploadDto file)
		{
			await using var stream = file.Content;

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, stream),
				Folder = "users/images",
				PublicId = Guid.NewGuid().ToString()
			};

			var result = await _cloudinary.UploadAsync(uploadParams);

			return result.SecureUrl.ToString();
		}

		public async Task<string> UploadFileAsync(FileUploadDto file)
		{
			await using var stream = file.Content;

			var uploadParams = new RawUploadParams
			{
				File = new FileDescription(file.FileName, stream),
				Folder = "users/cv",
				PublicId = Guid.NewGuid().ToString()
			};

			var result = await _cloudinary.UploadAsync(uploadParams);

			return result.SecureUrl.ToString();
		}
	}
}
