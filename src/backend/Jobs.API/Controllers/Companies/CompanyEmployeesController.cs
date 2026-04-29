using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Command.AddEmployee;
using Jobs.Application.Features.Companies.Command.RemoveEmployee;
using Jobs.Application.Features.Companies.Queries.GetCompanyEmployees;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	[Route("api/companies")]
    public class CompanyEmployeesController : ApiController
	{
		/// <summary>
		/// Retrieves a paginated list of employees for the currently authenticated company.
		/// </summary>
		/// <param name="page">Page number (default is 1).</param>
		/// <param name="pageSize">Number of records per page (default is 10).</param>
		/// <returns>
		/// Returns a paginated list of employees belonging to the authenticated company.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetCompanyEmployeesQuery.
		/// - Requires Companies_View permission.
		/// </remarks>
		/// <response code="200">Employees retrieved successfully.</response>
		/// <response code="400">Invalid pagination parameters.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// GET /api/companies/{companyId}/employees
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpGet("{companyId}/employees")]
		public async Task<IActionResult> GetCompanyEmployees(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null ) return Unauthorized();

			var query = new GetCompanyEmployeesQuery(companyId, page, pageSize);

			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Adds an employee to the currently authenticated company.
		/// </summary>
		/// <param name="employeeId">The unique identifier of the employee to be added.</param>
		/// <returns>
		/// Returns the result of the add operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via AddEmployeeCommand.
		/// - Requires Companies_View permission.
		/// </remarks>
		/// <response code="200">Employee added to company successfully.</response>
		/// <response code="400">Invalid employee ID supplied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Employee or company not found.</response>
		/// <response code="409">Employee is already assigned to the company.</response>
		// POST /api/companies/{companyId}/employees/{employeeId}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpPost("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> AddEmployeeToCompany(
			string employeeId)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null) return Unauthorized();

			var command = new AddEmployeeCommand(companyId, employeeId);

			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Removes an employee from the currently authenticated company.
		/// </summary>
		/// <param name="employeeId">The unique identifier of the employee to be removed.</param>
		/// <returns>
		/// Returns the result of the remove operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via RemoveEmployeeCommand.
		/// - Requires Companies_View permission (preferably Companies_Update or Companies_ManageEmployees).
		/// </remarks>
		/// <response code="200">Employee removed from company successfully.</response>
		/// <response code="400">Invalid employee ID supplied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Employee or company not found.</response>
		/// <response code="409">Employee is not associated with the company.</response>
		// DELETE /api/companies/{companyId}/employees/{employeeId}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpDelete("{companyId}/employees/{employeeId}")]
		public async Task<IActionResult> RemoveEmployeeFromCompany(
			string employeeId)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null) return Unauthorized();

			var result = await Sender.Send(new RemoveEmployeeCommand(companyId, employeeId));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

	}
}