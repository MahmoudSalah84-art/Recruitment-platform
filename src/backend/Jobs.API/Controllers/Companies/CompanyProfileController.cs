using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.Companies.Command.UpdateCompanyLogo;
using Jobs.Application.Features.Companies.Queries.GetCompanyDetails;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyProfileController : ApiController
	{
		/// <summary>
		/// Retrieves the profile of the currently authenticated company.
		/// </summary>
		/// <returns>
		/// Returns the company profile information.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetCompanyProfileQuery.
		/// - Requires Companies_View permission.
		/// </remarks>
		/// <response code="200">Company profile retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// GET /api/CompanyProfile
		[HttpGet()]
		[Authorize(Policy = Permissions.Companies_View)]
		public async Task<IActionResult> GetProfile()
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null) return Unauthorized();

			var result = await Sender.Send(new GetCompanyProfileQuery(companyId));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Uploads or updates the company profile image (logo).
		/// </summary>
		/// <param name="File">The image file to be uploaded.</param>
		/// <returns>
		/// Returns the result of the upload operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - The file is streamed and mapped to a FileUploadDto before processing.
		/// - Uses CQRS pattern via UpdateCompanyLogoCommand.
		/// - Requires Companies_Update permission.
		/// </remarks>
		/// <response code="200">Company image uploaded successfully.</response>
		/// <response code="400">Invalid file or unsupported format.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="413">File size too large.</response>
		/// <response code="415">Unsupported media type.</response>
		// POST http://localhost:5077/api/companyprofile/upload-companyimage
		[HttpPost("upload-companyimage")]
		[Authorize(Policy = Permissions.Companies_Update)]
		public async Task<IActionResult> UploadImage(IFormFile File )
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null ) return Unauthorized();

			var fileUploadDto = new FileUploadDto(File.FileName, File.ContentType,File.OpenReadStream());

			var result = await Sender.Send(new UpdateCompanyLogoCommand(companyId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}