using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.API.Extensions;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
using Jobs.Application.Features.CV.Query.GetMyResume;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.CVs
{
	public class CVController : ApiController
	{
		/// <summary>
		/// Uploads or updates the CV (resume) for the currently authenticated user.
		/// </summary>
		/// <param name="File">The CV file to be uploaded.</param>
		/// <returns>
		/// Returns the result of the create or update operation.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - The uploaded file is streamed and mapped to a FileUploadDto.
		/// - Uses CQRS pattern via CreateOrUpdateResumeCommand.
		/// - Requires CVs_Upload permission.
		/// </remarks>
		/// <response code="200">CV uploaded or updated successfully.</response>
		/// <response code="400">Invalid file or unsupported format.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="413">File size too large.</response>
		/// <response code="415">Unsupported media type.</response>
		// POST /api/UserProfile/CreateOrUpdate
		[HttpPost()]
		[Authorize(Policy = Permissions.CVs_Upload)]
		public async Task<IActionResult> CreateOrUpdate(IFormFile File)
		{

			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null ) return Unauthorized();

			var fileUploadDto = new FileUploadDto(File.FileName, File.ContentType, File.OpenReadStream());

			var result = await Sender.Send(new CreateOrUpdateResumeCommand(seekerId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves the CV (resume) of the currently authenticated user.
		/// </summary>
		/// <returns>
		/// Returns the user's uploaded CV details.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetMyResumeQuery.
		/// - Requires CVs_View permission.
		/// </remarks>
		/// <response code="200">CV retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">CV not found.</response>
		// GET http://localhost:5077/api/cv/userId=1
		[HttpGet()]
		[Authorize(Policy = Permissions.CVs_View)]
		public async Task<IActionResult> GetCv()
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null ) return Unauthorized();

			var result = await Sender.Send(new GetMyResumeQuery(seekerId));

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}
	}
}