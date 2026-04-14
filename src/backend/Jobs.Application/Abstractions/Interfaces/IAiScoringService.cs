using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IAiScoringService
	{
		Task<ParsedCvResponse> ParseCvAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

		Task<AiMatchResult> MatchJobAsync( Stream fileStream, string jobDescription, CancellationToken cancellationToken = default);

		//Task<IReadOnlyList<AiCandidateResult>> RankCandidatesAsync(
		//	IReadOnlyList<(string CvId, Stream CvPdf)> cvs, string jobDescription, CancellationToken cancellationToken = default);
		Task<IEnumerable<AiCandidateResult>> RankCandidatesAsync(
			List<(string CvId, Stream CvPdf)> cvs, string jobDescription, CancellationToken cancellationToken);
	}
}