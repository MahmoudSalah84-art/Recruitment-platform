using Jobs.API.Controllers.Abstractions;
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
			var result = await Sender.Send( new GetCompanyProfileQuery());

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}

		// PUT /api/CompanyProfile/{companyId}
		[HttpPut("{companyId}")]
		public async Task<IActionResult> UpdateProfile( [FromBody] UpdateCompanyCommand command)
		{
			var result = await Sender.Send( command );

			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}
	}
}