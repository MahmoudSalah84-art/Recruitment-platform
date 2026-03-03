using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Jobs.API.Controllers.AdminController
{
    public class AdminUsersController : ApiController
	{
		// GET: api/Users/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserProfileById(string id)
		{
			var query = new GetUserByIdQuery(id);
			var userProfile = await Sender.Send(query);

			if (userProfile == null)
				return NotFound(new { Message = "User not found" });

			return Ok(userProfile);
		}

		//GET: /api/Users
		[HttpGet()]
		public async Task<IActionResult> GetAllUsersProfiles()
		{
			var usersProfiles = await Sender.Send();
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}

//| Method |            Route                       | Description     |
//| ------ | -----------------------------          | --------------- |
//| GET    | `/ api / admin / users`                | Get all users   |
//| GET    | `/api/admin/users/{id}`                | Get single user |
//| PUT    | `/api/admin/users/{id}/ ban`           | Ban user        |
//| PUT    | `/ api / admin / users /{ id}/ unban`  | Unban user      |
//| DELETE | `/ api / admin / users /{ id}`         | Delete user     |
