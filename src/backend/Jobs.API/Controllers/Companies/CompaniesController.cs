using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Companies.Command.DeleteCompany;
using Jobs.Application.Features.Companies.Command.LoginCompany;
using Jobs.Application.Features.Companies.Command.Register;
using Jobs.Application.Features.Companies.Command.UpdateCompany;
using Jobs.Application.Features.Companies.Queries.GetAllCompanies;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	public sealed class CompaniesController : ApiController
	{
		// GET /api/companies
		[HttpGet]
		[Authorize(Policy = Permissions.Companies_View)]
		public async Task<IActionResult> GetAllCompanies(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? name = null,
			[FromQuery] string? industry = null)
		{
			var query = new GetAllCompaniesQuery(page, pageSize, name, industry);

			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}


		// GET /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCompanyById(string id)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != id) return Unauthorized();

			var query = new GetCompanyByIdQuery(id);

			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}


		// POST /api/companies/login-company
		[HttpPost("login-company")]
		public async Task<IActionResult> Login([FromBody] LoginCombanyCommand command, CancellationToken ct)
		{
			var result = await Sender.Send(command);

			//if (result.IsFailure)
			//{
			//	if (result.Error == "Auth.InvalidCredentials")
			//		return Unauthorized(result.Error);

			//	return BadRequest(result.Error);
			//}


			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		// POST /api/companies/register
		[HttpPost("register")]
		public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		// DELETE /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_Delete)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCompany(string id)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != id) return Unauthorized();

			await Sender.Send(new DeleteCompanyCommand(id));

			return NoContent(); // 204
		}

		// PUT /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_Update)]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyCommand command)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != id) return Unauthorized();

			if (id != command.CompanyId)
				return BadRequest("Route id and body id must match");

			await Sender.Send(command);

			return NoContent(); // 204
		}
	}
}









//// DELETE /api/companies/{id}
//[HttpDelete("{id:guid}")]
//public async Task<IActionResult> Delete(Guid id)
//{
//	await Sender.Send(new DeleteCompanyCommand(id));
//	return NoContent();
//}


//[HttpPost("register")] 
//public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyRequest request)
//{

//	var command = new RegisterCompanyCommand(
//		request.UserName,
//		request.Email,
//		request.Industry,
//		request.Country,
//		request.City,
//		request.Street,
//		request.BuildingNumber,
//		request.PostalCode,
//		request.Description,
//		request.Password,
//		request.ConfirmPassword
//	);

//	var result = await Sender.Send(command);

//	if (result.IsFailure) return Ok(result.Error);

//	return Ok(result.Value);
//}