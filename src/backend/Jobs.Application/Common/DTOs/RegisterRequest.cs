
namespace Jobs.Application.Common.DTOs
{
    public record RegisterRequest(
        string Id,
        string FirstName, 
        string LastName,
		string UserName,
		string Email,
        string Password,
        string ConfirmPassword)
    {}
}