using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Companies.Command.AddEmployee;
using Jobs.Application.Features.Companies.Command.DeleteCompany;
using Jobs.Application.Features.Companies.Command.LoginCompany;
using Jobs.Application.Features.Companies.Command.Register;
using Jobs.Application.Features.Companies.Command.RemoveEmployee;
using Jobs.Application.Features.Companies.Command.UpdateCompany;
using Jobs.Application.Features.Companies.Queries.GetAllCompanies;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Jobs.Application.Features.Companies.Queries.GetCompanyEmployees;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	public sealed class CompaniesController : ApiController
	{

		// GET /api/companies
		[HttpGet]
		public async Task<IActionResult> GetAllCompanies(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? name = null,
			[FromQuery] string? industry = null)
		{
			var query = new GetAllCompaniesQuery(page, pageSize, name, industry);

			var result = await Sender.Send(query);

			return Ok(result);
		}


		// GET /api/companies/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCompanyById(string id)
		{
			var query = new GetCompanyByIdQuery(id);

			var result = await Sender.Send(query);

			return Ok(result);
		}


		// POST /api/companies/login-company
		[HttpPost("login-company")]
		public async Task<IActionResult> Login([FromBody] LoginCombanyCommand command, CancellationToken ct)
		{
			var result = await Sender.Send(command);

			if (result.IsFailure)
			{
				if (result.Error == "Auth.InvalidCredentials")
					return Unauthorized(result.Error);

				return BadRequest(result.Error);
			}

			return Ok(result.Value);
		}

		// POST /api/companies/register
		[HttpPost("register")]
		public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyCommand command)
		{
			var result = await Sender.Send(command);

			return Ok(result);
		}

		// DELETE /api/companies/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCompany(string id)
		{
			await Sender.Send(new DeleteCompanyCommand(id));

			return NoContent(); // 204
		}

		// PUT /api/companies/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyCommand command)
		{
			if (id != command.CompanyId)
				return BadRequest("Route id and body id must match");

			await Sender.Send(command);

			return NoContent(); // 204
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