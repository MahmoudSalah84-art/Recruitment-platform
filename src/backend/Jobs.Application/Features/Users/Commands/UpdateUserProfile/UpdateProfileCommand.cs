
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Users.Commands.UpdateUserProfile
{
    public class UpdateProfileCommand : ICommand
	{
		public string UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; } = default!;
		public string? PhoneNumber { get; set; }
		public string? Bio { get; set; }
	}

}
