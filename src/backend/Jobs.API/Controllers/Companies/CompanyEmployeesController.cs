using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Companies.Command.AddEmployee;
using Jobs.Application.Features.Companies.Command.RemoveEmployee;
using Jobs.Application.Features.Companies.Queries.GetCompanyEmployees;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	[Route("api/companies")]
    public class CompanyEmployeesController : ApiController
	{

		// GET /api/companies/{companyId}/employees
		[HttpGet("{companyId}/employees")]
		public async Task<IActionResult> GetCompanyEmployees(
			string companyId,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			var query = new GetCompanyEmployeesQuery(companyId, page, pageSize);

			var result = await Sender.Send(query);

			return Ok(result);
		}



		// POST /api/companies/{companyId}/employees/{employeeId}
		[HttpPost("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> AddEmployeeToCompany(
			string companyId,
			string employeeId)
		{
			var command = new AddEmployeeCommand(companyId, employeeId);

			await Sender.Send(command);

			return NoContent(); // 204
		}


		

		// DELETE /api/companies/{companyId}/employees/{employeeId}
		[HttpDelete("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> RemoveEmployeeFromCompany(
			string companyId,
			string employeeId)
		{
			await Sender.Send(new RemoveEmployeeCommand(companyId, employeeId));

			return NoContent(); // 204
		}

	}
}