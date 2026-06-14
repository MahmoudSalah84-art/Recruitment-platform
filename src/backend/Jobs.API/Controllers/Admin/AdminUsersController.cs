using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//namespace Jobs.API.Controllers.AdminController
//{
//	public class AdminUsersController : ApiController
//	{
		// ===================== USERS =====================

	//	[HttpGet("users")]
	//	[Authorize(Policy = Permissions.Users_View)]
	//	public async Task<IActionResult> GetAllUsers(
	//		[FromQuery] GetAllUsersRequest request,
	//		CancellationToken cancellationToken)
	//	{
	//		var query = new GetAllUsersQuery(
	//			request.PageNumber,
	//			request.PageSize,
	//			request.SearchTerm,
	//			request.Role,
	//			request.IsActive);

	//		var result = await Sender.Send(query, cancellationToken);

	//		return result.Match(
	//			success => Ok(success),
	//			errors => Problem(errors));
	//	}

	//	[HttpGet("users/{userId:guid}")]
	//	[Authorize(Policy = Permissions.Admin_ViewUsers)]
	//	public async Task<IActionResult> GetUserDetails(
	//		Guid userId,
	//		CancellationToken cancellationToken)
	//	{
	//		var query = new GetUserDetailsQuery(userId);

	//		var result = await Sender.Send(query, cancellationToken);

	//		return result.Match(
	//			success => Ok(success),
	//			errors => Problem(errors));
	//	}

	//	[HttpDelete("users/{userId:guid}")]
	//	[Authorize(Policy = Permissions.Admin_ManageUsers)]
	//	public async Task<IActionResult> DeleteUser(
	//		Guid userId,
	//		CancellationToken cancellationToken)
	//	{
	//		var command = new DeleteUserCommand(userId);

	//		var result = await Sender.Send(command, cancellationToken);

	//		return result.Match(
	//			success => NoContent(),
	//			errors => Problem(errors));
	//	}

	//	[HttpPatch("users/{userId:guid}/disable")]
	//	[Authorize(Policy = Permissions.Admin_ManageUsers)]
	//	public async Task<IActionResult> DisableUser(
	//		Guid userId,
	//		CancellationToken cancellationToken)
	//	{
	//		var command = new DisableUserCommand(userId);

	//		var result = await Sender.Send(command, cancellationToken);

	//		return result.Match(
	//			success => NoContent(),
	//			errors => Problem(errors));
	//	}

	//	[HttpPatch("users/{userId:guid}/enable")]
	//	[Authorize(Policy = Permissions.Admin_ManageUsers)]
	//	public async Task<IActionResult> EnableUser(
	//		Guid userId,
	//		CancellationToken cancellationToken)
	//	{
	//		var command = new EnableUserCommand(userId);

	//		var result = await Sender.Send(command, cancellationToken);

	//		return result.Match(
	//			success => NoContent(),
	//			errors => Problem(errors));
	//	}

	//	[HttpPatch("users/{userId:guid}/role")]
	//	[Authorize(Policy = Permissions.Roles_Update)]
	//	public async Task<IActionResult> ChangeUserRole(
	//		Guid userId,
	//		[FromBody] ChangeUserRoleRequest request,
	//		CancellationToken cancellationToken)
	//	{
	//		var command = new ChangeUserRoleCommand(userId, request.NewRole);

	//		var result = await Sender.Send(command, cancellationToken);

	//		return result.Match(
	//			success => NoContent(),
	//			errors => Problem(errors));
	//	}

	//	// ===================== DASHBOARD =====================

	//	[HttpGet("dashboard/stats")]
	//	[Authorize(Policy = Permissions.Admin_ViewDashboard)]
	//	public async Task<IActionResult> GetDashboardStats(
	//		CancellationToken cancellationToken)
	//	{
	//		var query = new GetDashboardStatsQuery();

	//		var result = await Sender.Send(query, cancellationToken);

	//		return result.Match(
	//			success => Ok(success),
	//			errors => Problem(errors));
	//	}

	//	[HttpGet("dashboard/recent-activity")]
	//	[Authorize(Policy = Permissions.Admin_ViewDashboard)]
	//	public async Task<IActionResult> GetRecentActivity(
	//		[FromQuery] int count = 10,
	//		CancellationToken cancellationToken = default)
	//	{
	//		var query = new GetRecentActivityQuery(count);

	//		var result = await Sender.Send(query, cancellationToken);

	//		return result.Match(
	//			success => Ok(success),
	//			errors => Problem(errors));
	//	}
	//}
//}




//DELETE / api / admin / users /{ userId}           → DeleteUserCommand
//PATCH  /api/admin/users/{userId}/ disable   → DisableUserCommand
//PATCH  /api/admin/users/{userId}/ enable    → EnableUserCommand
//PATCH  /api/admin/users/{userId}/ role      → ChangeUserRoleCommand
//GET    /api/admin/users                    → GetAllUsersQuery (paginated)
//GET    /api/admin/users/{userId}           → GetUserDetailsQuery



//GET / api / admin / dashboard / stats          → GetDashboardStatsQuery
//GET    /api/admin/dashboard/recent-activity → GetRecentActivityQuery