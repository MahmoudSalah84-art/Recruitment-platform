
namespace Jobs.Infrastructure.Identity
{
	public class RefreshToken
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string TokenHash { get; set; }
		public DateTime ExpiryDate { get; set; }
		public bool IsRevoked { get; set; }
		public bool IsUsed { get; set; }
		public DateTime CreatedAt { get; set; }

		public string UserId { get; set; }
		public AppUser AppUser { get; set; } = null!;

		public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
		public bool IsActive => !IsRevoked && !IsUsed && !IsExpired; // at least one of these
	}
}
