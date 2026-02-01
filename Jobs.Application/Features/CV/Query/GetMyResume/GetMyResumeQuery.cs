using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.CV.Query.GetMyResume
{
	public record GetMyResumeQuery : IQuery<UserResumeDto>;

}
