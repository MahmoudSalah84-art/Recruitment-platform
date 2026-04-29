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
		/// <summary>
		/// Retrieves the profile of the currently authenticated user.
		/// </summary>
		/// <returns>
		/// Returns the user's profile information.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetUserByIdQuery.
		/// - This endpoint is scoped to the currently logged-in user (/me).
		/// </remarks>
		/// <response code="200">User profile retrieved successfully.</response>
		/// <response code="400">Failed to retrieve user profile.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
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

		/// <summary>
		/// Updates the profile of the currently authenticated user.
		/// </summary>
		/// <param name="command">The updated profile data.</param>
		/// <returns>
		/// Returns the result of the profile update operation.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via UpdateProfileCommand.
		/// - Requires Users_Update permission.
		/// - This endpoint updates personal user information (e.g. name, phone, etc.).
		/// </remarks>
		/// <response code="200">Profile updated successfully.</response>
		/// <response code="400">Invalid input data.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
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

		/// <summary>
		/// Uploads or updates the profile image of the currently authenticated user.
		/// </summary>
		/// <param name="File">The image file to be uploaded.</param>
		/// <returns>
		/// Returns the result of the image upload operation.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - The file is streamed and mapped to a FileUploadDto.
		/// - Uses CQRS pattern via UpdateUserImageCommand.
		/// - Requires Users_Update permission.
		/// </remarks>
		/// <response code="200">User image uploaded successfully.</response>
		/// <response code="400">Invalid file or unsupported format.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
		/// <response code="413">File size too large.</response>
		/// <response code="415">Unsupported media type.</response>
		// POST http://localhost:5077/api/userprofile/upload-userimage
		[HttpPost("upload-userimage")]
		[Authorize(Policy = Permissions.Users_Update)]
		public async Task<IActionResult> CreateOrUpdateUserImage(IFormFile File)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null ) return Unauthorized();

			var fileUploadDto = new FileUploadDto( File.FileName,  File.ContentType, File.OpenReadStream());

			var result = await Sender.Send(new UpdateUserImageCommand(seekerId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}