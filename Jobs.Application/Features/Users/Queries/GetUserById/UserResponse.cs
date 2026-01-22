using Jobs.Domain.Entities;
using Jobs.Domain.Enums;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
    public record UserResponse
    {
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string? PhoneNumber { get; set; }
		public UserRole Role { get; set; }
		public string? Bio { get; set; }
		public string? ProfileImage { get; set; } 
		public IReadOnlyCollection<UserSkill> Skills { get; set; } = default!;
		public IReadOnlyCollection<CV> CVs { get; set; } = default!;
	}
}