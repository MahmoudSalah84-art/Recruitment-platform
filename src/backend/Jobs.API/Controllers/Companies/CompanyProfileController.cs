using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Companies.Command.UpdateCompanyLogo;
using Jobs.Application.Features.Companies.Commands.UpdateCompany;
using Jobs.Application.Features.Companies.Commands.UpdateCompanyProfile;
using Jobs.Application.Features.Companies.Queries.GetCompanyDetails;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyProfileController : ApiController
	{
		// GET /api/CompanyProfile/{companyId}
		[HttpGet("{companyId}")]
		public async Task<IActionResult> GetProfile()
		{
			var result = await Sender.Send(new GetCompanyProfileQuery());

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}


		// POST /api/CompanyProfile/upload-image
		[HttpPost("upload-image")]
		public async Task<IActionResult> UploadImage(UpdateCompanyLogoCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);

		}
	}
}