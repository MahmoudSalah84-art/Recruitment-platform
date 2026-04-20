
namespace Jobs.Application.Common.Emails
{
	public abstract record EmailMessage(string To, string Subject)
	{
		public abstract string BuildBody();
	}
}
