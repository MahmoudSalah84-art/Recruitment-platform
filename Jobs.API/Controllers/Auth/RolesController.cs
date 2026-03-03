using Jobs.Application.Features.Identity.Query.GetAllRoles;
using Jobs.Application.Features.Identity.Query.GetRoleById;
using Jobs.Application.Features.Identity.Query.GetUsersInRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Auth
{
	[Authorize(Roles = "Admin")]
	[ApiController]
	[Route("api/[controller]")]
	public class RolesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RolesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// الحصول على جميع الأدوار
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetAllRoles()
		{
			var query = new GetAllRolesQuery();
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// الحصول على دور حسب المعرف
		/// </summary>
		[HttpGet("{roleId}")]
		public async Task<IActionResult> GetRoleById(Guid roleId)
		{
			var query = new GetRoleByIdQuery { RoleId = roleId };
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return NotFound(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// الحصول على المستخدمين في دور معين
		/// </summary>
		[HttpGet("{roleId}/users")]
		public async Task<IActionResult> GetUsersInRole(Guid roleId)
		{
			var query = new GetUsersInRoleQuery { RoleId = roleId };
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return NotFound(new { message = result.Message });

			return Ok(new { data = result.Data });
		}
	}
}
