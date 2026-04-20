using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Users.Commands.UpdateUserProfile;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Users
{
	public class UserProfileController : ApiController
	{

		// GET: api/UserProfile/me
		[HttpGet("me")]
		public async Task<IActionResult> GetMyProfile()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			//if (CompanyId == null || CompanyId != id) return Unauthorized();

			var query = new GetUserByIdQuery(userId);
			var result = await Sender.Send(query);

			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		// PUT: api/UserProfile
		[HttpPut("me")]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
		{
			var userIdClaim = User.FindFirst("sub")?.Value;
			if (string.IsNullOrEmpty(userIdClaim))
				return Unauthorized();

			var result = await Sender.Send(command);

			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}



	}
}