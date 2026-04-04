using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IAiServiceATS
	{
		Task<ParsedCvResponse> ParseCvAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

		Task<AiMatchResult> MatchJobAsync( Stream fileStream, string jobDescription, CancellationToken cancellationToken = default);

		//Task<AIRankingResponse> RankCandidateAsync(byte[] cvFile, string fileName, string jobDescription, CancellationToken cancellationToken = default);
		//Task<IReadOnlyList<AIRankingResponse>> RankCandidatesAsync( IReadOnlyList<(string CvId, Stream CvPdf)> cvs, string jobDescription, CancellationToken cancellationToken = default);
	}
}