using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.DTOs;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Jobs.Infrastructure.Services
{
	public class AiScoringService : IAiScoringService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<AiScoringService> _logger;

		public AiScoringService(HttpClient httpClient, ILogger<AiScoringService> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public async Task<ParsedCvResponse> ParseCvAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
		{
			using var content = new MultipartFormDataContent();

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

			content.Add(fileContent, "file", fileName);

			var response = await _httpClient.PostAsync("http://ai-service/api/parse-cv", content);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<ParsedCvResponse>();

			return result;
		}
		
		public async Task<AiMatchResult> MatchJobAsync(Stream fileStream, string jobDescription, CancellationToken cancellationToken = default)
		{
			using var content = new MultipartFormDataContent();
			content.Add(new StreamContent(fileStream), "cv", "cv.pdf");
			content.Add(new StringContent(jobDescription), "job_description");

			var response = await _httpClient.PostAsync( "/api/ai/match-job", content, cancellationToken);

			response.EnsureSuccessStatusCode();

			var result = await response.Content
				.ReadFromJsonAsync<AiMatchResult>(cancellationToken: cancellationToken);

			return new AiMatchResult(
				MatchScore: result!.MatchScore,
				Decision: result.Decision,
				MissingSkills: result.MissingSkills,
				Explanation: result.Explanation);
		}

		

		 public async Task<IEnumerable<AiCandidateResult>> RankCandidatesAsync( List<(string CvId, Stream CvPdf)> cvs, string jobDescription,
					CancellationToken cancellationToken = default)
				{
					using var form = new MultipartFormDataContent();

					foreach (var (cvId, cvPdfTask) in cvs)
					{
						var cvStream = cvPdfTask;
						form.Add(new StreamContent(cvStream), "cvs", $"{cvId}.pdf");
					}

					form.Add(new StringContent(jobDescription), "job_description");

					var response = await _httpClient.PostAsync(
						"/api/ai/rank-candidates", form, cancellationToken);

					response.EnsureSuccessStatusCode();

					var results = await response.Content
						.ReadFromJsonAsync<List<AiRankCandidateResponse>>(cancellationToken: cancellationToken);

					return results!.Select((r, i) => new AiCandidateResult(
						CvId: cvs[i].CvId,
						MatchScore: r.MatchScore,
						Decision: r.Decision,
						MissingSkills: r.MissingSkills,
						Explanation: r.Explanation))
						.ToList();
				}

	





		// Response DTOs
		private sealed record AiMatchJobResponse(
			[property: JsonPropertyName("match_score")] int MatchScore,
			[property: JsonPropertyName("decision")] string Decision,
			[property: JsonPropertyName("missing_skills")] List<string> MissingSkills,
			[property: JsonPropertyName("explanation")] string Explanation);

		private sealed record AiRankCandidateResponse(
			[property: JsonPropertyName("match_score")] int MatchScore,
			[property: JsonPropertyName("decision")] string Decision,
			[property: JsonPropertyName("missing_skills")] List<string> MissingSkills,
			[property: JsonPropertyName("explanation")] string Explanation);
	}
}
