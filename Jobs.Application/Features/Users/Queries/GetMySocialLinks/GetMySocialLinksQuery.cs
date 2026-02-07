using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Queries.GetMySocialLinks
{
	public record GetMySocialLinksQuery : IQuery<SocialLinksDto>;
}
