
namespace Jobs.Application.Common.Emails
{
	public record EmailConfirmationMessage( string To, string UserName, string ConfirmationLink) : EmailMessage( To, "Confirm Your Email Address")
	{
		public override string BuildBody() => $"""
        <html><body style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
          <div style="background:#f4f4f4;padding:30px;border-radius:8px">
            <h2 style="color:#2c3e50">Welcome, {UserName}!</h2>{To}
            <p>Thank you for registering. Please confirm your email address by clicking the button below.</p>
            <a href="{ConfirmationLink}"
               style="background:#3498db;color:white;padding:12px 24px;
                      text-decoration:none;border-radius:5px;display:inline-block;margin:16px 0">
              Confirm Email
            </a>
            <p style="color:#999;font-size:12px">This link expires in 24 hours. If you did not register, ignore this email.</p>
          </div>
        </body></html>
        """;
	}
}