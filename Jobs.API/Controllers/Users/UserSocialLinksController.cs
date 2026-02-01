using Jobs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace Jobs.API.Controllers.Users
{
	public class UserSocialLinksController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UserSocialLinksController(IMediator mediator) => _mediator = mediator;

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateSocialLinksCommand command)
		{
			var result = await _mediator.Send(command);
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}

//Method, Route, Description
//GET,/api/social-links, جلب روابط التواصل الاجتماعي الخاصة بالمستخدم.
//PUT,/api/social-links, تحديث الروابط (عادة تكون مجموعة روابط واحدة يتم تحديثها معاً).


//GET / api / users / me / social - links
//POST / api / users / me / social - links
//PUT / api / users / me / social - links /{ id}
//DELETE / api / users / me / social - links /{ id}
