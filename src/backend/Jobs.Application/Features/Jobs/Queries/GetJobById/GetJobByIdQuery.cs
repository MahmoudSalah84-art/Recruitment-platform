
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Jobs.Queries.GetJobById
{
	public record GetJobByIdQuery(string JobId) : IQuery<JobResponse>;
}
