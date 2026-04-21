using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.DTOs;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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

		//public async Task<AiMatchResult> MatchJobAsync(Stream fileStream, string jobDescription, CancellationToken cancellationToken = default)
		//{
		//	using var content = new MultipartFormDataContent();
		//	content.Add(new StreamContent(fileStream), "cv", "cv.pdf");
		//	content.Add(new StringContent(jobDescription), "job_description");

		//	var response = await _httpClient.PostAsync("https://jeanne-unaddled-shawnee.ngrok-free.dev/docs", content, cancellationToken);

		//	//https://jeanne-unaddled-shawnee.ngrok-free.dev/docs
		//	response.EnsureSuccessStatusCode();

		//	var result = await response.Content
		//		.ReadFromJsonAsync<AiMatchResult>(cancellationToken: cancellationToken);

		//	return new AiMatchResult(
		//		MatchScore: result!.MatchScore,
		//		Decision: result.Decision,
		//		MissingSkills: result.MissingSkills,
		//		Explanation: result.Explanation);
		//}
		public async Task<AiMatchResult> MatchJobAsync(
	Stream fileStream,
	string jobDescription,
	CancellationToken cancellationToken = default)
		{
			var cvId = 1.ToString();

			using var content = new MultipartFormDataContent();

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType =
				new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

			content.Add(fileContent, "cv", "cv.pdf");
			content.Add(new StringContent(cvId), "cv_id");
			content.Add(new StringContent(jobDescription), "job_description");

			var url = "https://jeanne-unaddled-shawnee.ngrok-free.dev/api/ai/match-job";
			var response = await _httpClient.PostAsync(url, content, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
				throw new HttpRequestException(
					$"AI Service Error: {response.StatusCode}, Content: {errorContent}");
			}

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
			};

			var raw = await response.Content
				.ReadFromJsonAsync<AiMatchRawResponse>(jsonOptions, cancellationToken);

			return new AiMatchResult(
				MatchScore: raw!.MatchScore,
				Decision: raw.Decision,
				MatchedSkills: raw.Skills.Matched,
				MissingSkills: raw.Skills.Missing,
				Explanation: raw.Explanation,
				Details: new AiMatchDetails(
					RawScore: raw.Details.RawScore,
					SemanticScore: raw.Details.SemanticScore,
					Skills: new AiSkillsDetail(
						Score: raw.Details.Skills.Score,
						MatchedSkills: raw.Details.Skills.MatchedSkills,
						MissingSkills: raw.Details.Skills.MissingSkills),
					Title: new AiTitleDetail(raw.Details.Title.Score),
					Experience: new AiExperienceDetail(
						Score: raw.Details.Experience.Score,
						DeltaYears: raw.Details.Experience.DeltaYears)));
		}





		//public async Task<AiMatchResult> MatchJobAsync(Stream fileStream, string jobDescription, CancellationToken cancellationToken = default)
		//{
		//	// temprory cvId
		//	var cvId = 1.ToString();


		//	using var content = new MultipartFormDataContent();


		//	var fileContent = new StreamContent(fileStream);
		//	// تأكد من تحديد الـ Media Type إذا كان السيرفر يدقق عليه
		//	fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
		//	content.Add(fileContent, "cv", "cv.pdf");

		//	content.Add(new StringContent(cvId), "cv_id");

		//	content.Add(new StringContent(jobDescription), "job_description");

		//	var url = "https://jeanne-unaddled-shawnee.ngrok-free.dev/api/ai/match-job";

		//	var response = await _httpClient.PostAsync(url, content, cancellationToken);

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		var errorContent = await response.Content.ReadAsStringAsync();
		//		throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
		//	}

		//	var result = await response.Content
		//		.ReadFromJsonAsync<AiMatchResult>(cancellationToken: cancellationToken);

		//	return new AiMatchResult(
		//		MatchScore: result!.MatchScore,
		//		Decision: result.Decision,
		//		MissingSkills: result.MissingSkills,
		//		Explanation: result.Explanation
		//	);
		//}


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
						"https://jeanne-unaddled-shawnee.ngrok-free.dev/docs", form, cancellationToken);
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




	// Raw response records للـ deserialization فقط — مش بتتعرض للباقي
	file sealed record AiMatchRawResponse(
		[property: JsonPropertyName("cv_id")] string CvId,
		[property: JsonPropertyName("match_score")] double MatchScore,
		[property: JsonPropertyName("decision")] string Decision,
		[property: JsonPropertyName("skills")] AiSkillsRaw Skills,
		[property: JsonPropertyName("explanation")] string Explanation,
		[property: JsonPropertyName("details")] AiDetailsRaw Details);

	file sealed record AiSkillsRaw(
		[property: JsonPropertyName("matched")] IReadOnlyList<string> Matched,
		[property: JsonPropertyName("missing")] IReadOnlyList<string> Missing);

	file sealed record AiDetailsRaw(
		[property: JsonPropertyName("raw_score")] double RawScore,
		[property: JsonPropertyName("semantic_score")] double SemanticScore,
		[property: JsonPropertyName("skills")] AiDetailsSkillsRaw Skills,
		[property: JsonPropertyName("title")] AiDetailsTitleRaw Title,
		[property: JsonPropertyName("experience")] AiDetailsExperienceRaw Experience);

	file sealed record AiDetailsSkillsRaw(
		[property: JsonPropertyName("score")] double Score,
		[property: JsonPropertyName("matched_skills")] IReadOnlyList<string> MatchedSkills,
		[property: JsonPropertyName("missing_skills")] IReadOnlyList<string> MissingSkills);

	file sealed record AiDetailsTitleRaw(
		[property: JsonPropertyName("score")] double Score);

	file sealed record AiDetailsExperienceRaw(
		[property: JsonPropertyName("score")] double Score,
		[property: JsonPropertyName("delta_years")] double DeltaYears);
}
