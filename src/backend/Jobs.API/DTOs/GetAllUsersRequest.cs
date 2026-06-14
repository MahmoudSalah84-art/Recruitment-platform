namespace Jobs.API.DTOs
{
	public record GetAllUsersRequest(
	int PageNumber = 1,
	int PageSize = 10,
	string? SearchTerm = null,
	string? Role = null,
	bool? IsActive = null);
}
