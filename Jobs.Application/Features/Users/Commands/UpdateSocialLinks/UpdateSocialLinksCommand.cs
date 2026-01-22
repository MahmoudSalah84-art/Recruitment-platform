using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UpdateSocialLinks
{
	public record UpdateSocialLinksCommand : IRequest<Result>
	{
		public string? LinkedInUrl { get; set; }
		public string? GitHubUrl { get; set; }
		public string? PortfolioUrl { get; set; }
	}
}
