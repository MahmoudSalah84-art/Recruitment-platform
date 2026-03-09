using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.ValueObjects;
using static System.Net.Mime.MediaTypeNames;

namespace Jobs.Domain.Entities
{
    public class CV : AggregateRoot , ISoftDelete
	{

		// ========== Properties ==========
		public string UserId { get; private set; }

        public string Title { get; private set; } = string.Empty;
		public FilePath FilePath { get; private set; }
		public string? SummaryText { get; private set; }

		public ParsedData? ParsedData { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }



		// Navigation Properties
		public User User { get; set; }

		private readonly List<JobApplication> _applications = new();
		public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();


		private readonly List<CVJobRecommendation> _cVJobRecommendations = new();
		public IReadOnlyCollection<CVJobRecommendation> CVJobRecommendations => _cVJobRecommendations.AsReadOnly();



		// ========== Constructor ==========
		private CV() { }
		public CV(string userId, string title, string file, string? summary)
		{
			CheckRule(new NotNullRule<string>(userId));
			CheckRule(new NotNullRule<string>(file));
			CheckRule(new NotEmptyRule(title, title));

			UserId = userId;
			Title = title;
			FilePath.Create(file);
			SummaryText = summary;
			CreatedAt = DateTime.UtcNow;
			UpdatedAt = CreatedAt;
			AddEvent(new CvUploadedEvent(this));
			//AddEvent(new CvUploadedEvent(this.Id, userId, title));
		}

		// ========== Behaviors ==========

		// ==================== File ====================
		public void UpdateFile(FilePath newFilePath, string title)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new DomainException("CV title is required.");

			FilePath = newFilePath ?? throw new ArgumentNullException(nameof(newFilePath));
			Title = title;

			// لما الفايل يتغير، الـ ParsedData القديمة بتبقى invalid
			ParsedData = null;

			//RaiseDomainEvent(new CVFileUpdatedDomainEvent(Id, UserId));
		}

		// ==================== Summary ====================
		public void UpdateSummary(string? summaryText)
		{
			SummaryText = summaryText;
		}

		// ==================== Parsed Data ====================
		public void AttachParsedData(ParsedData parsedData)
		{
			ParsedData = parsedData ?? throw new ArgumentNullException(nameof(parsedData));

			//RaiseDomainEvent(new CVParsedDomainEvent(Id, UserId));
		}

		public void ClearParsedData()
		{
			if (ParsedData is null)
				throw new DomainException("No parsed data to clear.");

			ParsedData = null;
		}


		// ==================== Job Applications ====================
		public void LinkApplication(JobApplication application)
		{
			if (application is null)
				throw new ArgumentNullException(nameof(application));

			if (_applications.Any(a => a.Id == application.Id))
				throw new DomainException("Application already linked to this CV.");

			_applications.Add(application);
		}

		public void UnlinkApplication(string applicationId)
		{
			var application = _applications.FirstOrDefault(a => a.Id == applicationId);

			if (application is null)
				throw new DomainException("Application not found in this CV.");

			_applications.Remove(application);
		}
		// ==================== Job Recommendations ====================
		public void AddJobRecommendation(CVJobRecommendation recommendation)
		{
			if (recommendation is null)
				throw new ArgumentNullException(nameof(recommendation));

			if (ParsedData is null)
				throw new DomainException("CV must be parsed before generating recommendations.");

			if (_cVJobRecommendations.Any(r => r.JobId == recommendation.JobId))
				throw new DomainException("This job is already recommended for this CV.");

			_cVJobRecommendations.Add(recommendation);
		}

		public void ClearJobRecommendations()
		{
			_cVJobRecommendations.Clear();
		}

		public void RefreshJobRecommendations(IEnumerable<CVJobRecommendation> newRecommendations)
		{
			if (ParsedData is null)
				throw new DomainException("CV must be parsed before refreshing recommendations.");

			_cVJobRecommendations.Clear();

			foreach (var recommendation in newRecommendations)
				_cVJobRecommendations.Add(recommendation);

			//RaiseDomainEvent(new CVRecommendationsRefreshedDomainEvent(Id, UserId));
		}

		// ==================== Soft Delete ====================
		public void Delete()
		{
			if (IsDeleted)
				throw new DomainException("CV is already deleted.");

			if (_applications.Any(a => a.Status == ApplicationStatus.Pending))
				throw new DomainException("Cannot delete a CV with pending applications.");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;

			//RaiseDomainEvent(new CVDeletedDomainEvent(Id, UserId));
		}

		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("CV is not deleted.");

			IsDeleted = false;
			DeletedAt = null;

			//RaiseDomainEvent(new CVRestoredDomainEvent(Id, UserId));
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}





