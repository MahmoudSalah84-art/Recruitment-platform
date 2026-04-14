using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
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
		// GET /api/CompanyProfile
		[HttpGet()]
		[Authorize(Policy = Permissions.Companies_View)]
		public async Task<IActionResult> GetProfile()
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null) return Unauthorized();


			var result = await Sender.Send(new GetCompanyProfileQuery(CompanyId));

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}


		// POST /api/CompanyProfile/upload-companyimage
		[HttpPost("upload-companyimage")]
		[Authorize(Policy = Permissions.Companies_Update)]
		public async Task<IActionResult> UploadImage(UpdateCompanyLogoCommand command)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != command.CompanyId) return Unauthorized();


			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}