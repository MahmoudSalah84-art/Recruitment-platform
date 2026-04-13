using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Companies.Command.AddEmployee;
using Jobs.Application.Features.Companies.Command.RemoveEmployee;
using Jobs.Application.Features.Companies.Queries.GetCompanyEmployees;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	[Route("api/companies")]
    public class CompanyEmployeesController : ApiController
	{

		// GET /api/companies/{companyId}/employees
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpGet("{companyId}/employees")]
		public async Task<IActionResult> GetCompanyEmployees(
			string companyId,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != companyId) return Unauthorized();

			var query = new GetCompanyEmployeesQuery(companyId, page, pageSize);

			var result = await Sender.Send(query);

			return Ok(result);
		}



		// POST /api/companies/{companyId}/employees/{employeeId}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpPost("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> AddEmployeeToCompany(
			string companyId,
			string employeeId)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != companyId) return Unauthorized();

			var command = new AddEmployeeCommand(companyId, employeeId);

			await Sender.Send(command);

			return NoContent(); // 204
		}




		// DELETE /api/companies/{companyId}/employees/{employeeId}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpDelete("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> RemoveEmployeeFromCompany(
			string companyId,
			string employeeId)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != companyId) return Unauthorized();

			await Sender.Send(new RemoveEmployeeCommand(companyId, employeeId));

			return NoContent(); // 204
		}

	}
}