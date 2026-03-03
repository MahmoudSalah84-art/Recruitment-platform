using Jobs.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Jobs.Infrastructure.Identity
{
	public class AppUser : IdentityUser
	{

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string FullName => $"{FirstName} {LastName}";

		public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

	}
}
