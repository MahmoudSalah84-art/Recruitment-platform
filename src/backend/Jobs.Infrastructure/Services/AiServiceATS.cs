using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Jobs.Infrastructure.Services
{
	public class AiServiceATS : IAiServiceATS
	{
		private readonly HttpClient _httpClient;

		public AiServiceATS(HttpClient httpClient)
		{
			_httpClient = httpClient;
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

		public async Task<AIRankingResponse> RankCandidateAsync(byte[] cvFile, string fileName, string jobDescription)
		{
			using var content = new MultipartFormDataContent();
			content.Add(new ByteArrayContent(cvFile), "cvs", fileName);
			content.Add(new StringContent(jobDescription), "job_description");

			var response = await _httpClient.PostAsync("api/ai/rank-candidates", content);
			// هنا بتعمل Deserialize للـ JSON اللي راجع (زي اللي في الصورة)
			return await response.Content.ReadFromJsonAsync<AIRankingResponse>();
		}

		
	}
}
