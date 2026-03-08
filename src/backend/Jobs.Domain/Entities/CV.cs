using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
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
		public void SetParsedData(ParsedData parsed)
		{
			CheckRule(new NotNullRule<ParsedData>(parsed));

			ParsedData = parsed ;
			AddEvent(new CvParsedEvent(this));
		}
		
		public void Update(string title, string file, string summary)
		{
			Title = title;
			FilePath.Create(file);
			SummaryText = summary;
			UpdatedAt = DateTime.UtcNow;
			AddEvent(new CvUploadedEvent(this));
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}





