using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.Emails
{
	public record WelcomeMessage(
	string To, string UserName, string Role) : EmailMessage (To, $"Welcome to Jobs Portal, {UserName}!")
	{
		public override string BuildBody() => $"""
        <html><body style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
          <div style="background:#f4f4f4;padding:30px;border-radius:8px">
            <h2 style="color:#27ae60">Welcome aboard, {UserName}! 🎉</h2>
            <p>Your account has been confirmed. You're now registered as <strong>{Role}</strong>.</p>
            <p>You can now log in and start using the platform.</p>
          </div>
        </body></html>
        """;
	}
}
