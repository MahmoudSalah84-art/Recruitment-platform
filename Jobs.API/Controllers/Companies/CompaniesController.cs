using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Companies.Commands.CreateCompany;
using Jobs.Application.Features.Companies.Commands.DeleteCompany;
using Jobs.Application.Features.Companies.Queries.GetCompanies;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	[ApiController]
	public sealed class CompaniesController : ApiController
	{
		// GET /api/companies
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await Sender.Send(new GetCompaniesQuery());
			return Ok(result);
		}

		// GET /api/companies/{id}
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await Sender.Send(new GetCompanyByIdQuery(id));
			return result is null ? NotFound() : Ok(result);
		}

		// POST /api/companies
		[HttpPost]
		public async Task<IActionResult> Create(CreateCompanyCommand command)
		{
			var result = await Sender.Send(command);
			return CreatedAtAction(nameof(GetById), new { id = result }, null);
		}

		// DELETE /api/companies/{id}
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await Sender.Send(new DeleteCompanyCommand(id));
			return NoContent();
		}
	}
}
