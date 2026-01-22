using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Users.Commands.UpdateUserProfile;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Jobs.API.Controllers.Users
{
    public class UserProfileController: ApiController
	{

		
		//UserProfileController, GET,/api/profile, جلب بيانات الملف الشخصي للمستخدم الحالي (Logged-in User).
		//, PUT,/api/profile, تحديث بيانات الملف الشخصي (يستدعي UpdateProfileCommand).

		[HttpGet("me")]
		public async Task<IActionResult> GetMyProfile()
		{
			var userIdClaim = User.FindFirst("sub")?.Value;
			if (string.IsNullOrEmpty(userIdClaim))
				return Unauthorized();

			var query = new GetUserByIdQuery(userIdClaim);
			var userProfile = await Sender.Send(query);

			return Ok(userProfile);
		}



		// PUT: api/UserProfile
		[HttpPut]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
		{
			var userIdClaim = User.FindFirst("sub")?.Value;
			if (string.IsNullOrEmpty(userIdClaim))
				return Unauthorized();

			var result = await Sender.Send(command);

			if (result.IsFailure)
				return BadRequest(result.Error);

			return NoContent();
		}
		

	//// POST: api/UserProfile/change-password
	//[HttpPost("change-password")]
	//public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
	//{
	//	var userIdClaim = User.FindFirst("sub")?.Value;
	//	if (string.IsNullOrEmpty(userIdClaim))
	//		return Unauthorized();

	//	command.UserId = int.Parse(userIdClaim);
	//	var result = await _mediator.Send(command);

	//	if (!result)
	//		return BadRequest(new { Message = "Failed to change password" });

	//	return NoContent();
	//}


	//[HttpPost("upload-picture")]
	//public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile picture)
	//{
	//	var userId = int.Parse(User.FindFirst("sub")!.Value);
	//	var url = await _mediator.Send(new UploadProfilePictureCommand { UserId = userId, Picture = picture });
	//	return url == null ? BadRequest() : Ok(new { Url = url });
	//}



	//[HttpGet("applications")]
	//public async Task<IActionResult> GetMyApplications()
	//{
	//	var userId = int.Parse(User.FindFirst("sub")!.Value);
	//	var apps = await _mediator.Send(new GetMyApplicationsQuery(userId));
	//	return Ok(apps);
	//}


	//[HttpGet("confirm-email")] // GET: api/Account/confirm-email?userId=...&token=...
	//public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
	//{
	//	var user = await _userManager.FindByIdAsync(userId);
	//	if (user == null)
	//		return BadRequest("Invalid user!!!");

	//	var result = await _userManager.ConfirmEmailAsync(user, token);
	//	if (!result.Succeeded)
	//		return BadRequest("Email confirmation failed !!!!!");

	//	return Ok("Email confirmation is successed");

	//}



	//[HttpPost("resend-verification")]
	//public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailCommand command)
	//{
	//	var result = await _mediator.Send(command);
	//	return result ? Ok() : BadRequest();
	//}



	//[HttpDelete]
	//public async Task<IActionResult> DeleteProfile()
	//{
	//	var userId = int.Parse(User.FindFirst("sub")!.Value);
	//	var result = await _mediator.Send(new DeleteProfileCommand { UserId = userId });
	//	return result ? NoContent() : BadRequest();
	//}

}
}
