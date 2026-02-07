using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Users.Commands.DeleteSocialLink;
using Jobs.Application.Features.Users.Commands.UpdateSinglePlatform;
using Jobs.Application.Features.Users.Commands.UpdateSocialLinks;
using Jobs.Application.Features.Users.Queries.GetMySocialLinks;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	[Route("api/users/me/social-links")]
	public class UserSocialLinksController : ApiController
	{
		// GET /api/users/me/social-links
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await Sender.Send(new GetMySocialLinksQuery());
			return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
		}

		// POST /api/users/me/social-links
		[HttpPost]
		public async Task<IActionResult> CreateOrUpdate(
			[FromBody] UpdateMySocialLinksCommand command)
		{
			var result = await Sender.Send(command);
			return result.IsSuccess ? Ok() : BadRequest(result.Error);
		}

		// PUT /api/users/me/social-links/{platform}
		[HttpPut("{platform}")]
		public async Task<IActionResult> Update( string platform, [FromBody] UpdateSingleSocialLinkCommand command)
		{
			if (platform != command.Platform)
				return BadRequest("platform in URL does not match platform in body.");

			var result = await Sender.Send(command);
			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}

		// DELETE /api/users/me/social-links/{platform}
		[HttpDelete("{platform}")]
		public async Task<IActionResult> Delete(string platform)
		{
			var result = await Sender.Send( new DeleteSocialLinkCommand(platform));

			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}
	}

}
