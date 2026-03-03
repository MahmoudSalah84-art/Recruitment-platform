
namespace Jobs.Infrastructure.Services
{
	public interface IFileStorageService
	{
		Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken ct = default);
		Task<Stream?> GetFileAsync(string path, CancellationToken ct = default);
		Task DeleteFileAsync(string path, CancellationToken ct = default);
	}

	/// <summary>
	/// Local disk storage implementation (example). Replace with S3 / Azure Blob for production.
	/// </summary>
	public class LocalFileStorageService : IFileStorageService
	{
		private readonly string _basePath;

		public LocalFileStorageService(string basePath)
		{
			_basePath = basePath;
			if (!Directory.Exists(_basePath)) Directory.CreateDirectory(_basePath);
		}

		public async Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken ct = default)
		{
			var path = Path.Combine(_basePath, fileName);
			using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
			await content.CopyToAsync(fs, ct);
			return path;
		}

		public Task<Stream?> GetFileAsync(string path, CancellationToken ct = default)
		{
			if (!File.Exists(path)) return Task.FromResult<Stream?>(null);
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return Task.FromResult<Stream?>(stream);
		}

		public Task DeleteFileAsync(string path, CancellationToken ct = default)
		{
			if (File.Exists(path)) File.Delete(path);
			return Task.CompletedTask;
		}
	}
}
