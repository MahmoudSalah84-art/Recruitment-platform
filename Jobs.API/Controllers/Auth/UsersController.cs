using Jobs.Application.Features.Identity.Commands.AssignClaim;
using Jobs.Application.Features.Identity.Commands.AssignRole;
using Jobs.Application.Features.Identity.Commands.RemoveClaim;
using Jobs.Application.Features.Identity.Commands.RemoveRole;
using Jobs.Application.Features.Identity.Query.GetAllUsers;
using Jobs.Application.Features.Identity.Query.GetUserRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Auth
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// الحصول على جميع المستخدمين (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = null, [FromQuery] bool? isActive = null)
		{
			var query = new GetAllUsersQuery
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				SearchTerm = searchTerm,
				IsActive = isActive
			};

			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// الحصول على مستخدم حسب المعرف (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserById(Guid userId)
		{
			var query = new GetUserByIdQuery { UserId = userId };
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return NotFound(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// الحصول على أدوار المستخدم (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpGet("{userId}/roles")]
		public async Task<IActionResult> GetUserRoles(Guid userId)
		{
			var query = new GetUserRolesQuery { UserId = userId };
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return NotFound(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// تعيين دور للمستخدم (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost("{userId}/roles")]
		public async Task<IActionResult> AssignRole(Guid userId, [FromBody] AssignRoleDto dto)
		{
			var command = new AssignRoleCommand
			{
				UserId = userId,
				RoleId = dto.RoleId
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// إزالة دور من المستخدم (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpDelete("{userId}/roles/{roleId}")]
		public async Task<IActionResult> RemoveRole(Guid userId, Guid roleId)
		{
			var command = new RemoveRoleCommand
			{
				UserId = userId,
				RoleId = roleId
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// تعيين ادعاء للمستخدم (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost("{userId}/claims")]
		public async Task<IActionResult> AssignClaim(Guid userId, [FromBody] AssignClaimDto dto)
		{
			var command = new AssignClaimCommand
			{
				UserId = userId,
				ClaimType = dto.ClaimType,
				ClaimValue = dto.ClaimValue
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// إزالة ادعاء من المستخدم (Admin فقط)
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpDelete("{userId}/claims")]
		public async Task<IActionResult> RemoveClaim(Guid userId, [FromQuery] string claimType, [FromQuery] string claimValue)
		{
			var command = new RemoveClaimCommand
			{
				UserId = userId,
				ClaimType = claimType,
				ClaimValue = claimValue
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}
	}
}
