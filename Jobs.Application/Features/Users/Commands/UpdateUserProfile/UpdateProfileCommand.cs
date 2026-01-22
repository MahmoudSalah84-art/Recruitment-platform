
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Commands.UpdateUserProfile
{
    public class UpdateProfileCommand : ICommand
	{
		public string FullName { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string? PhoneNumber { get; set; }
		public string? Bio { get; set; }
		public string? ProfilePictureUrl { get; set; }
	}
}
