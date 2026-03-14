using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Skills.Commands.DeleteUserSkill;
using Jobs.Application.Features.Skills.Queries.GetUserSkills;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserSkillsController : ApiController
	{
		// GET /api/userSkills
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await Sender.Send(new GetUserSkillsQuery());

			return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
		}

		//// POST /api/userSkills
		//[HttpPost]
		//public async Task<IActionResult> Add(AddUserSkillCommand command)
		//{
		//	var result = await Sender.Send(command);

		//	return result.IsSuccess ? Ok() : BadRequest(result.Error);
		//}

		// DELETE /api/userSkills/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await Sender.Send(new DeleteUserSkillCommand(id));

			return result.IsSuccess ? NoContent() : NotFound(result.Error);
		}
	}
}
