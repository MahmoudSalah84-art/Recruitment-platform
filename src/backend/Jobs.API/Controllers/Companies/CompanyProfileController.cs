using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.API.Extensions;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.Companies.Command.UpdateCompanyLogo;
using Jobs.Application.Features.Companies.Queries.GetCompanyDetails;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
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


		// POST http://localhost:5077/api/companyprofile/upload-companyimage
		[HttpPost("upload-companyimage")]
		[Authorize(Policy = Permissions.Companies_Update)]
		public async Task<IActionResult> UploadImage(CreateOrUpdateFileDto command)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != command.UserId) return Unauthorized();

			var fileUploadDto = new FileUploadDto(command.File.FileName, command.File.ContentType, command.File.OpenReadStream());

			var result = await Sender.Send(new UpdateCompanyLogoCommand(command.UserId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}


	
	}
}