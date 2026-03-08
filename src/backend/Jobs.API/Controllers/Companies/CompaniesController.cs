using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.Companies.Command.LoginCompany;
using Jobs.Application.Features.Companies.Command.Register;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	[ApiController]
	public sealed class CompaniesController : ApiController
	{

		//// GET /api/companies
		//[HttpGet]
		//public async Task<IActionResult> GetAll()
		//{
		//	var result = await Sender.Send(new GetCompaniesQuery());
		//	return Ok(result);
		//}

		//// GET /api/companies/{id}
		//[HttpGet("{id:guid}")]
		//public async Task<IActionResult> GetById(Guid id)
		//{
		//	var result = await Sender.Send(new GetCompanyByIdQuery(id));
		//	return result is null ? NotFound() : Ok(result);
		//}


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
				public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyRequest request)
				{
					FileUploadDto? fileDto = default;

					if (request.Image != null)
					{
						using var memoryStream = new MemoryStream();
						await request.Image.CopyToAsync(memoryStream);
						fileDto = new FileUploadDto(
							request.Image.FileName,
							request.Image.ContentType,
							memoryStream
						);
					}

					var command = new RegisterCompanyCommand(
						request.UserName,
						request.Email,
						request.Industry,
						request.Country,
						request.City,
						request.Street,
						request.BuildingNumber,
						request.PostalCode,
						request.Description,
						fileDto,
						request.Password,
						request.ConfirmPassword
					);

					var result = await Sender.Send(command);

					if (result.IsFailure) return Ok(result.Error);

					return Ok(result.Value);
				}
	}



	//// DELETE /api/companies/{id}
	//[HttpDelete("{id:guid}")]
	//public async Task<IActionResult> Delete(Guid id)
	//{
	//	await Sender.Send(new DeleteCompanyCommand(id));
	//	return NoContent();
	//}
}