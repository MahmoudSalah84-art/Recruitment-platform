
namespace Jobs.Application.Common.DTOs
{

	// AiMatchResult.cs
	public sealed record AiMatchResult(
		double MatchScore,
		string Decision,
		IReadOnlyList<string> MatchedSkills,
		IReadOnlyList<string> MissingSkills,
		string Explanation,
		AiMatchDetails Details);

	public sealed record AiMatchDetails(
		double RawScore,
		double SemanticScore,
		AiSkillsDetail Skills,
		AiTitleDetail Title,
		AiExperienceDetail Experience);

	public sealed record AiSkillsDetail(
		double Score,
		IReadOnlyList<string> MatchedSkills,
		IReadOnlyList<string> MissingSkills);

	public sealed record AiTitleDetail(double Score);

	public sealed record AiExperienceDetail(double Score, double DeltaYears);
}
