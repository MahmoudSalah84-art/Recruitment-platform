
namespace Jobs.Application.Common.DTOs
{
	public record UserDto
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsEmailVerified { get; set; }
		public bool IsActive { get; set; }
		public List<string> Roles { get; set; } = new();
	}
}
