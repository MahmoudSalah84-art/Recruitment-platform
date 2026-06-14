using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class CV : AggregateRoot 
	{

		// ========== Properties ==========
		public string UserId { get; private set; }

        public string Title { get; private set; } = string.Empty;
		public FilePath FilePath { get; private set; }
		public string? SummaryText { get; private set; }

		public ParsedData? ParsedData { get; private set; }


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
			FilePath = FilePath.Create(file);
			SummaryText = summary;
			
			AddEvent(new CvUploadedEvent(userId));
		}

		// ========== Behaviors ==========

		// ==================== File ====================
		public void UpdateFile( string newFilePath, string title)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new DomainException("CV title is required.");

			FilePath = FilePath.Create(newFilePath);
			Title = title;

			ParsedData = null;

			AddEvent(new CvUploadedEvent(UserId));
			//AddEvent(new CvUploadedEvent( UserId));
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

			//RaiseDomainEvent(new CVParsedClearedDomainEvent(Id, UserId));
		}

		// ==================== Soft Delete ====================


		//public void Restore()
		//{
		//	if (!IsDeleted)
		//		throw new DomainException("CV is not deleted.");

		//	IsDeleted = false;
		//	DeletedAt = null;

		//	//RaiseDomainEvent(new CVRestoredDomainEvent(Id, UserId));
		//}

	}
}





