
namespace Jobs.Application.Common.Emails
{
	public abstract record EmailMessage(string To, string Subject)
	{
		public string To { get; init; } = string.Empty;
		public string Subject { get; init; } = string.Empty;

		public abstract string BuildBody();
	}


	//public abstract record EmailMessage
	//{

	//	public abstract string BuildBody();
	//}
}
