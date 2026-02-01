using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Jobs.API.Controllers.Users
{
    public class UsersController : ApiController
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


//GET / api / users /{ id}
//DELETE / api / users /{ id}
