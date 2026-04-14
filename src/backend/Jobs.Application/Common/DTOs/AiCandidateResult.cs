
namespace Jobs.Application.Common.DTOs
{
	public sealed record AiCandidateResult(
		string CvId,
		int MatchScore,
		string Decision,
		IReadOnlyList<string> MissingSkills,
		string Explanation);
}
