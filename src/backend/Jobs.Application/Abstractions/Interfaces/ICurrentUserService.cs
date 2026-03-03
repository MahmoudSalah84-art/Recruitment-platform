namespace Jobs.Application.Abstractions.Interfaces
{
	public interface ICurrentUserService
	{
		string? UserId { get; }
		string? Email { get; }
		bool IsAuthenticated { get; }
		IEnumerable<string> Roles { get; }
	}

}
