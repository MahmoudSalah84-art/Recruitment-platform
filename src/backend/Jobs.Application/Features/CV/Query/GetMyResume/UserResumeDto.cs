
using Jobs.Domain.ValueObjects;

namespace Jobs.Application.Features.CV.Query.GetMyResume
{
	public class UserResumeDto
	{
		public required string Title { get; set; }
		public required FilePath FilePath { get; set; }
		public string? Summary { get; set; }

	}
}
