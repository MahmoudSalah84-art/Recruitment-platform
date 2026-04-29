using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Companies.Command.DeleteCompany;
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

		/// <summary>
		/// Retrieves a paginated list of companies with optional filtering.
		/// </summary>
		/// <param name="page">Page number (default is 1).</param>
		/// <param name="pageSize">Number of records per page (default is 10).</param>
		/// <param name="name">Optional filter by company name.</param>
		/// <param name="industry">Optional filter by industry.</param>
		/// <returns>
		/// Returns a paginated list of companies based on the provided filters.
		/// </returns>
		/// <remarks>
		/// Requires Companies_View permission.
		/// </remarks>
		/// <response code="200">Companies retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		// GET /api/companies
		[HttpGet]
		[Authorize(Policy = Permissions.Companies_View)]
		public async Task<IActionResult> GetAllCompanies(
			[FromQuery] int page = 1, [FromQuery] int pageSize = 10,
			[FromQuery] string? name = null, [FromQuery] string? industry = null)
		{
			var query = new GetAllCompaniesQuery(page, pageSize, name, industry);

			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves a specific company by its unique identifier.
		/// </summary>
		/// <param name="companyid">The unique identifier of the company.</param>
		/// <returns>
		/// Returns the company details if found.
		/// </returns>
		/// <remarks>
		/// Requires Companies_View permission.
		/// </remarks>
		/// <response code="200">Company retrieved successfully.</response>
		/// <response code="400">Invalid company ID supplied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// GET /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_View)]
		[HttpGet("{companyid}")]
		public async Task<IActionResult> GetCompanyById(string companyid)
		{
			//string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var query = new GetCompanyByIdQuery(companyid);

			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}


		/// <summary>
		/// Registers a new company account.
		/// </summary>
		/// <param name="command">The company registration data.</param>
		/// <returns>
		/// Returns authentication tokens and company details upon successful registration.
		/// </returns>
		/// <remarks>
		/// This endpoint creates a new company account and issues authentication tokens.
		/// Access and refresh tokens are stored securely in HttpOnly cookies.
		/// </remarks>
		/// <response code="200">Company registered successfully.</response>
		/// <response code="400">Invalid input data.</response>
		/// <response code="409">Company already exists.</response>
		//// POST /api/companies/register
		[HttpPost("register")]
		public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse();

			if (response.IsSuccess && response.Data is not null)
			{
				var accessToken = response.Data.AccessToken;
				var refreshToken = response.Data.RefreshToken;
				var accessTokenExpiry = response.Data.AccessTokenExpiry;

				// Access Token
				Response.Cookies.Append("accessToken", accessToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = accessTokenExpiry
				});

				// Refresh Token
				Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(7)
				});
			}

			return StatusCode(response.StatusCode, response);
		}


		/// <summary>
		/// Deletes the currently authenticated company account.
		/// </summary>
		/// <returns>
		/// Returns the result of the delete operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via DeleteCompanyCommand.
		/// - Requires Companies_Delete permission.
		/// </remarks>
		/// <response code="200">Company deleted successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// DELETE /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_Delete)]
		[HttpDelete]
		public async Task<IActionResult> DeleteCompany()
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new DeleteCompanyCommand(CompanyId));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Updates the currently authenticated company's information.
		/// </summary>
		/// <param name="command">The updated company data.</param>
		/// <returns>
		/// Returns the result of the update operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Ensures that the authenticated company can only update its own data.
		/// - Uses CQRS pattern via UpdateCompanyCommand.
		/// - Requires Companies_Update permission.
		/// </remarks>
		/// <response code="200">Company updated successfully.</response>
		/// <response code="400">Invalid input data.</response>
		/// <response code="401">Unauthorized - user is not authenticated or trying to update another company.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// PUT /api/companies/{id}
		[Authorize(Policy = Permissions.Companies_Update)]
		[HttpPut]
		public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyCommand command)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != command.CompanyId) return Unauthorized();

			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}