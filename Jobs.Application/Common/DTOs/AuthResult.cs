
namespace Jobs.Application.Common.DTOs
{
	public record AuthResult(bool IsSuccess, string? AccessToken, string? RefreshToken, IEnumerable<string>? Errors) { }
}
