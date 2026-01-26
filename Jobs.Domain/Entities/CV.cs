using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Rules;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class CV : AggregateRoot , ISoftDelete
	{

		// ========== Properties ==========
		public Guid UserId { get; private set; }
		public User User { get; set; }

        public string Title { get; private set; } = string.Empty;
		public FilePath FilePath { get; private set; }
		public string? SummaryText { get; private set; }

		public ParsedData? ParsedData { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }

		// ========== Constructor ==========
		private CV() { }
		public CV(Guid userId, string title, FilePath file)
		{
			CheckRule(new NotNullRule<Guid>(userId));
			CheckRule(new NotNullRule<FilePath>(file));
			CheckRule(new NotEmptyRule(title, title));
			

			UserId = userId;
			Title = title;
			FilePath = file;
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
		
		public void SetSummary(string summary)
		{
			SummaryText = summary;
			
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}





