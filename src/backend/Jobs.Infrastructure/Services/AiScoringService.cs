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
		private readonly string _url = "https://jeanne-unaddled-shawnee.ngrok-free.dev/api/ai/match-job";

		public AiScoringService(HttpClient httpClient, ILogger<AiScoringService> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public async Task<ParsedCvResponse?> ParseCvAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
		{
			using var content = new MultipartFormDataContent();

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

			content.Add(fileContent, "file", fileName);

			var response = await _httpClient.PostAsync(_url, content, cancellationToken);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<ParsedCvResponse>();
			if(result is null)
			{
				_logger.LogError("Failed to parse CV. Response content was empty or not in expected format.");
				
			}

			return result;
		}


		public async Task<AiMatchResult> MatchJobAsync( Stream fileStream, string jobDescription, CancellationToken cancellationToken = default)
		{
			var cvId = 1.ToString();

			using var content = new MultipartFormDataContent();

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

			content.Add(fileContent, "cv", "cv.pdf");
			content.Add(new StringContent(cvId), "cv_id");
			content.Add(new StringContent(jobDescription), "job_description");
						

			var response = await _httpClient.PostAsync(_url, content, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
				_logger.LogError($"Failed to match job. Error: {response.StatusCode}, Content: {errorContent}");
				throw new HttpRequestException($"AI scoring service returned {(int)response.StatusCode} {response.ReasonPhrase}: {errorContent}");
			}

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
			};



			
			// Read the response as string first so we can log non-JSON responses and avoid
			// System.Text.Json throwing a JsonReaderException with an unhelpful message.
			var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

			AiMatchRawResponse raw;
			try
			{
				raw = JsonSerializer.Deserialize<AiMatchRawResponse>(responseString, jsonOptions)
					  ?? throw new JsonException("Deserialized AI match response was null.");
			}
			catch (JsonException ex)
			{
				_logger.LogError(ex, "Failed to deserialize AI scoring response. Response content: {Response}", responseString);
				throw;
			}

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
						DeltaYears: raw.Details.Experience.DeltaYears
					)
				)
			);
		}

		public async Task<IEnumerable<AiCandidateResult>> RankCandidatesAsync( List<(string CvId, Stream CvPdf)> cvs, string jobDescription, CancellationToken cancellationToken = default)
		{
			using var content = new MultipartFormDataContent();

			foreach (var (cvId, cvPdfTask) in cvs)
			{
				var cvStream = cvPdfTask;
				content.Add(new StreamContent(cvStream), "cvs", $"{cvId}.pdf");
			}

			content.Add(new StringContent(jobDescription), "job_description");

			var response = await _httpClient.PostAsync(_url, content, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
				_logger.LogError($"Failed to Rank CVs.  Error: {response.StatusCode}, Content: {errorContent}");
			}

			var results = await response.Content
				.ReadFromJsonAsync<List<AiRankCandidateResponse>>(cancellationToken);

			return results!.Select((r, i) => new AiCandidateResult(
				CvId: cvs[i].CvId,
				MatchScore: r.MatchScore,
				Decision: r.Decision,
				MissingSkills: r.MissingSkills,
				Explanation: r.Explanation))
				.ToList();

		}

		private sealed record AiRankCandidateResponse(
			[property: JsonPropertyName("match_score")] int MatchScore,
			[property: JsonPropertyName("decision")] string Decision,
			[property: JsonPropertyName("missing_skills")] List<string> MissingSkills,
			[property: JsonPropertyName("explanation")] string Explanation);
	}


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
