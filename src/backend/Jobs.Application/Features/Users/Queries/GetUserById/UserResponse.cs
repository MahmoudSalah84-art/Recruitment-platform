using Jobs.Domain.Entities;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
    public record UserResponse
    {
		public string Id { get; set; }
		public string FirsName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string? PhoneNumber { get; set; }
		public string? Bio { get; set; }
		public string? ProfileImage { get; set; } 
		public IReadOnlyCollection<UserSkill> Skills { get; set; } = default!;
	}
}