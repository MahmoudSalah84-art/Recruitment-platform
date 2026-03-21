using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Companies.Command.LoginCompany;
using Jobs.Application.Features.Users.Commands.Login;
using Jobs.Application.Features.Users.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserController : ApiController
	{


		// Post: api/User/register
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
		{
			var result = await Sender.Send(command, ct);

			if (result.IsFailure)
			{
				return BadRequest(result.Error);
			}

			return Ok(result.Value);
		}


		// POST api/User/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
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
	}
}
