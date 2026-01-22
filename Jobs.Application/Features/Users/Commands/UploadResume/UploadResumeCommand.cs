using Jobs.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UploadResume
{
	public record UploadResumeCommand : IRequest<Result<string>>
	{
		public required IFormFile File { get; set; }
	}
}
