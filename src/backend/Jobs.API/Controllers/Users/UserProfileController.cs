using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.API.Extensions;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
using Jobs.Application.Features.Users.Commands.UpdateUserImage;
using Jobs.Application.Features.Users.Commands.UpdateUserProfile;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
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

		// PUT http://localhost:5077/api/UserProfile
		[HttpPut("me")]
		[Authorize(Policy = Permissions.Users_Update)]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);

		}

		// POST http://localhost:5077/api/userprofile/upload-userimage
		[HttpPost("upload-userimage")]
		[Authorize(Policy = Permissions.Users_Update)]
		public async Task<IActionResult> CreateOrUpdateUserImage(CreateOrUpdateFileDto command)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null || seekerId != command.UserId) return Unauthorized();

			var fileUploadDto = new FileUploadDto(command.File.FileName, command.File.ContentType, command.File.OpenReadStream());

			var result = await Sender.Send(new UpdateUserImageCommand(command.UserId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}